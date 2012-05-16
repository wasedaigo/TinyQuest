require 'rubygems'
require "google_spreadsheet"
require "json"

LANGS = ["ja", "en"]
$lookuptable = {}

# Add data to look up table
def add_data_to_lookup_table(title, ws)
  $lookuptable[title] = {}
  
  # Skim through headers
  if (title == "ENum")
    for col in 1..ws.num_cols
      header = ws[1, col]
      first_element = ws[2, col]
      $lookuptable[title][header] = {first_element => 0}
    end
  else
    $lookuptable[title]["None"] = 0
  end
  
  # Read entity
  for row in 3..ws.num_rows
    id = row - 2
    for col in 1..ws.num_cols
      header = ws[1, col]
      arr = header.split("_")
      if LANGS.include?(arr.last)
        lang = arr.last
        if (lang == "ja") 
          $lookuptable[title][ws[row, col]] = id
        end
      else
        # Build look up table
        if (title == "ENum")
          $lookuptable[title][header] = $lookuptable[title][header] ? $lookuptable[title][header] : {}
          $lookuptable[title][header][ws[row, col]] = id
        end
      end
    end
  end
end

# Get master
def get_master(title, ws)
  has_data = false
  data = []
  localized_text = {}
  
  for row in 3..ws.num_rows
    id = row - 2
    row_data = {}
    for col in 1..ws.num_cols
      header = ws[1, col]
      break if header == ""
      arr = header.split("_")
      if LANGS.include?(arr.last)
        lang = arr.last
        localized_text[lang] = localized_text[lang] ? localized_text[lang] : {}
        localized_text[lang][id] = localized_text[lang][id] ? localized_text[lang][id] : {}
        localized_text[lang][id][arr[0]] = ws[row, col]
      else
        row_data[header] =  ws[row, col]
        #has_data = true
      end
    end

    row_data["id"] = id
    data << row_data
  end
  
  unless has_data
    #data = nil
  end
  return {:data => data, :localized_text => localized_text}
end

USER = ARGV[0]
PASS = ARGV[1]
MASTER_OUTPUT_PATH = ARGV[2]
LOCALIZE_OUTPUT_PATH = MASTER_OUTPUT_PATH + "/Localize"

SPREAD_SHEET_KEY = '0ArQTGa1g85ANdEViRS1ZNlozcVhFUTRETXZRQkR3SUE'

session = GoogleSpreadsheet.login(USER, PASS)

p "# Connecting..."
worksheets = session.spreadsheet_by_url("https://docs.google.com/spreadsheet/ccc?key=" + SPREAD_SHEET_KEY).worksheets
p "# Connected"


localized_text = {}
data = {}

p "# Processing..."
worksheets.each do |worksheet|
  ["Zone", "ZoneMonster",  "Monster", "MonsterDropItem", "Recipe", "Material", "Weapon", "Skill", "CompositeSkill", "ENum"].each do |title|
    if worksheet.title == title
      add_data_to_lookup_table(title, worksheet)
      if (title != "ENum")
        obj = get_master(title, worksheet)
        if obj[:localized_text]
          localized_text[title] = obj[:localized_text]
        end
        if obj[:data]
          data[title] = obj[:data]
        end
      end
      
      p " [#{title}]"
    end
  end
end

p "# Processed"

### 
#   Resolve relations
###

p "# Resolving relations"
data["Zone"].each do |value|
  value["dependency"] = $lookuptable["Zone"][value["dependency"]]
end

data["Monster"].each do |value|
  value["growthType"] = $lookuptable["ENum"]["growthType"][value["growthType"]]
  value["weapon1"] = $lookuptable["Weapon"][value["weapon1"]]
  value["weapon2"] = $lookuptable["Weapon"][value["weapon2"]]
  value["weapon3"] = $lookuptable["Weapon"][value["weapon3"]]
end

data["ZoneMonster"].each do |value|
  value["zone"] = $lookuptable["Zone"][value["zone"]]
  value["monster"] = $lookuptable["Monster"][value["monster"]]
  is_boss = $lookuptable["ENum"]["bool"][value["isBoss"]]
  value["isBoss"] = is_boss == 1 ? true : false
end

data["MonsterDropItem"].each do |value|
  value["monster"] = $lookuptable["Monster"][value["monster"]]
  type = value["type"]
  value["type"] = $lookuptable["ENum"]["itemType"][value["type"]]
  value["drop"] = $lookuptable[type][value["drop"]]
end

data["Recipe"].each do |value|
  value["weapon"] = $lookuptable["Weapon"][value["weapon"]]
  value["material"] = $lookuptable["Material"][value["material"]]
end

data["Weapon"].each do |value|
  value["category"] = $lookuptable["ENum"]["weaponCategory"][value["category"]]
  value["growthType"] = $lookuptable["ENum"]["growthType"][value["growthType"]]
  value["skill1"] = $lookuptable["Skill"][value["skill1"]]
  value["skill2"] = $lookuptable["Skill"][value["skill2"]]
  value["skill3"] = $lookuptable["Skill"][value["skill3"]]
  value["rarity"] = $lookuptable["ENum"]["rarity"][value["rarity"]]
  value["userType"] = $lookuptable["ENum"]["userType"][value["userType"]]
end

data["Skill"].each do |value|
  value["element"] = $lookuptable["ENum"]["element"][value["element"]]
  value["attribute"] = $lookuptable["ENum"]["attribute"][value["attribute"]]
  value["buff"] = $lookuptable["ENum"]["buff"][value["buff"]]
end

data["CompositeSkill"].each do |value|
  value["element"] = $lookuptable["ENum"]["element"][value["element"]]
  value["attribute"] = $lookuptable["ENum"]["attribute"][value["attribute"]]
  value["buff"] = $lookuptable["ENum"]["buff"][value["buff"]]
  value["baseSkill1"] = $lookuptable["Skill"][value["baseSkill1"]]
  value["baseSkill2"] = $lookuptable["Skill"][value["baseSkill2"]]
  value["baseSkill3"] = $lookuptable["Skill"][value["baseSkill3"]]
end

p "# Resolved"

newData = {}
# Unstringify interger and float
data.each do |worksheet_name, worksheet|
  worksheet.each do |obj|
    obj.each do |key, value|
      if value =~ /\d+\.\d+/
       obj[key] = value.to_f
      elsif  value =~ /\d/
        obj[key] = value.to_i
      end
    end
  end
  
  newData[worksheet_name + "s"] = data[worksheet_name]
end

output = {"version" => 1, "data" => newData}

# Save it as a file
File.open(MASTER_OUTPUT_PATH + "/Master.txt", 'w') do |f|
    f.write(JSON.pretty_generate(output))
end

# Save it as a file
LANGS.each do |lang|
  result = {}
  
  localized_text.each do |title, content|
    result[title] = content[lang]
  end
  
  File.open(LOCALIZE_OUTPUT_PATH + "/#{lang}.txt", 'w') do |f|
      f.write(JSON.pretty_generate(result))
  end
end
