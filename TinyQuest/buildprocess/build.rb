require 'rubygems'
require 'json'

def build_animation_file(filename)
    data = ""
    File.open(filename, 'r') do |f|
       data = f.read
    end
    json = JSON.parse(data)

    result = {}
    result["timelines"] = parse_timelines(json["keyframes"])
    
    outputFilename = filename.split(".")[0] + ".json"
    File.open(outputFilename, 'w') do |f|
       f.write(result.to_json)
    end
end

def parse_timelines(timelines)
    result = []
    timelines.each do |timelineIndex, timeline|
        result << parse_timeline(timeline)
    end
    
    return result
end

def parse_timeline(timeline)
    result = {
        "alpha" => [],
        "position" => [],
        "rotation" => [],
        "scale" => [],
        "hue" => [],
        "source" => []
    }

    latestSourceData = {"textureRect" => nil, "sourcePath" => nil}
    timeline.each do |keyframe_set|
        parse_keyframes(latestSourceData, keyframe_set, result)
    end

    return result
end

def parse_keyframes(latestSourceData, keyframe_set, result)
    frameNo = keyframe_set["frameNo"]

    if (keyframe_set["isEmpty"])
        # When an empty keyframe appears, duration of all latest keyframes have to be updated.
        result.each do |key, obj|
            if key != "source"
                last_obj = obj.last
                if last_obj && last_obj["duration"] == nil
                    latestSourceData["textureRect"] = nil
                    latestSourceData["sourcePath"] = nil
                    #last_obj["duration"] = frameNo - last_obj["frameNo"]
                    last_obj["tween"] = false;
                end
            end
        end
        
        result["source"] << {"frameNo" => frameNo, "path" => ""}
    else
        # Add keyframe for each attribute in this keyframe set
        createAttributeKey(result, "alpha", keyframe_set, frameNo, 1.0)
        createAttributeKey(result, "position", keyframe_set, frameNo, [0, 0])
        createAttributeKey(result, "rotation", keyframe_set, frameNo, 0)
        createAttributeKey(result, "scale", keyframe_set, frameNo, [1, 1])
        createAttributeKey(result, "hue", keyframe_set, frameNo, [0, 0, 0])

        # Source is somewhat special (Like, no lenear tween)
        if (latestSourceData["textureRect"] != keyframe_set["textureRect"]) || (latestSourceData["sourcePath"] != keyframe_set["sourcePath"] )
            latestSourceData["textureRect"] = keyframe_set["textureRect"]
            latestSourceData["sourcePath"] = keyframe_set["sourcePath"]
            if keyframe_set["textureRect"]
                result["source"] << {"frameNo" => frameNo, "path" => keyframe_set["sourcePath"], "rect" => keyframe_set["textureRect"]}
            end
        end
        
        # setup duration
=begin
        result.each do |key, obj|
            if key != "source"
                if (obj.count > 1)
                    curr_keyframe = obj[obj.count - 1]
                    if (curr_keyframe["frameNo"] == frameNo)
                        prev_keyframe = obj[obj.count - 2]
                        unless prev_keyframe["duration"]
                            prev_keyframe["duration"] = curr_keyframe["frameNo"] - prev_keyframe["frameNo"]
                        end
                    end
                end
            end
        end
=end
    end
end

def createAttributeKey(result, key, keyframe_set, frameNo, defaultValue)
    data = {}

    value = keyframe_set[key]
    tweenType = keyframe_set[key + "Tween"]

    if tweenType == "linear"
        data["tween"] = true
    end
      
    if tweenType && tweenType != "none"
        data["frameNo"] = frameNo
        data["value"] = (value != nil) ? value : defaultValue
    end

    unless data.empty?
        result[key] << data
    end
end

filename = ARGV[0]
if filename
    build_animation_file(filename)
end
