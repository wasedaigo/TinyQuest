require 'rubygems'
require 'json'

INPUT_PATH = "buildprocess/rawassets/animations"
OUTPUT_PATH = "static/assets/animations"

def build_animation_file(filename)
    data = ""
    File.open(filename, 'r') do |f|
       data = f.read
    end
    json = JSON.parse(data)

    result = {}
    result["timelines"], result["dependencies"] = parse_timelines(json["keyframes"])
    
    return result
end

def parse_timelines(timelines)
    result = []
    dependencies = {"images"=>[], "animations"=>[]}
    timelines.each do |timelineIndex, timeline|
        result << parse_timeline(timeline, dependencies)
    end
    
    return result, dependencies
end

def parse_timeline(timeline, dependencies)
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
        parse_keyframes(latestSourceData, keyframe_set, result, dependencies)
    end

    if (!result["source"].empty? && result["source"].last["id"] == "") 
        result["source"].pop();
    end

    return result
end

def parse_keyframes(latestSourceData, keyframe_set, result, dependencies)
    frameNo = keyframe_set["frameNo"]

    if (keyframe_set["isEmpty"])
        # When an empty keyframe appears, duration of all latest keyframes have to be updated.
        result.each do |key, obj|
			last_obj = obj.last
			if last_obj && last_obj["duration"] == nil
				last_obj["duration"] = frameNo - last_obj["frameNo"]
				if key != "source"
					last_obj["tween"] = false;
				end
			end
        end
		latestSourceData["textureRect"] = nil
		latestSourceData["sourcePath"] = nil
        result["source"] << {"frameNo" => frameNo, "id" => ""}
    else
        # Add keyframe for each attribute in this keyframe set
        createAttributeKey(result, "alpha", keyframe_set, frameNo, 1.0)
        createAttributeKey(result, "position", keyframe_set, frameNo, [0, 0])
        createAttributeKey(result, "rotation", keyframe_set, frameNo, 0)
        createAttributeKey(result, "scale", keyframe_set, frameNo, [1, 1])
        createAttributeKey(result, "hue", keyframe_set, frameNo, [0, 0, 0])

        # Source is somewhat special (Like, no lenear tween)
        rectDiff = keyframe_set["textureRect"] == nil || latestSourceData["textureRect"] != keyframe_set["textureRect"]
        pathDiff = latestSourceData["sourcePath"] != keyframe_set["sourcePath"]
        if rectDiff || pathDiff
            latestSourceData["textureRect"] = keyframe_set["textureRect"]
            latestSourceData["sourcePath"] = keyframe_set["sourcePath"]
            
            # Relative
            relativeToTarget = false
            if (keyframe_set["positionType"] == "relativeToTarget")
                relativeToTarget = true
            end

            id = keyframe_set["sourcePath"].split(".")[0]
            ext = keyframe_set["sourcePath"].split(".")[1]
            sourceKeyFrame = {"frameNo" => frameNo, "id" => id, "relative" => relativeToTarget}
            if ext == "png"
                unless dependencies["images"].index(id)
                    dependencies["images"] << id
                end
                sourceKeyFrame["type"] = "image"
                sourceKeyFrame["rect"] = keyframe_set["textureRect"] ? keyframe_set["textureRect"] : [0,0,0,0]
                # Anchor
                anchor = [0, 0]
                if (keyframe_set["center"])
                    anchor = keyframe_set["center"]
                end
                sourceKeyFrame["anchor"] = anchor
            else
                unless dependencies["animations"].index(id)
                    dependencies["animations"] << id
                end
                sourceKeyFrame["type"] = "animation"
                if keyframe_set["emitter"]
                    sourceKeyFrame["emitter"] = true
                    sourceKeyFrame["maxEmitAngle"] = keyframe_set["maxEmitAngle"]
                    sourceKeyFrame["maxEmitSpeed"] = keyframe_set["maxEmitSpeed"]
                    sourceKeyFrame["minEmitAngle"] = keyframe_set["minEmitAngle"]
                    sourceKeyFrame["minEmitSpeed"] = keyframe_set["minEmitSpeed"]
                else
                    sourceKeyFrame["emitter"] = false
                end
            end
            
            result["source"] << sourceKeyFrame
        end

        # setup duration
        result.each do |key, obj|
			if (obj.count > 1)
				curr_keyframe = obj[obj.count - 1]
				if (curr_keyframe["frameNo"] == frameNo)
					prev_keyframe = obj[obj.count - 2]
					unless prev_keyframe["duration"]
						prev_keyframe["duration"] = curr_keyframe["frameNo"] - prev_keyframe["frameNo"]
						if key != "source"
							prev_keyframe["endValue"] = curr_keyframe["startValue"]
						end
					end
				end
			end
        end

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
        data["startValue"] = (value != nil) ? value : defaultValue
        data["endValue"] = data["startValue"]
    end

    unless data.empty?
        result[key] << data
    end
end

def retrieveAllImageDependencies(animation_data, root_id)
    root = animation_data[root_id]
    if root["processed"]
        return root["data"]["dependencies"]["images"]
    else
        image_dependencies = root["data"]["dependencies"]["images"]
        root["data"]["dependencies"]["animations"].each do |child_id|
            child_image_dependencies = retrieveAllImageDependencies(animation_data, child_id)
            image_dependencies = image_dependencies | child_image_dependencies
        end

        root["data"]["dependencies"]["images"] = image_dependencies
        root["processed"] = true
        return image_dependencies
    end
end 

animation_data = {}
list = Dir["#{INPUT_PATH}/**/*.ani"]
list.each do |filename|
    # Convert Splash animation file into lightweight json object
    result = build_animation_file(filename)
    

    # Create directories
    id = filename.gsub(%r{^#{INPUT_PATH}/}, "").split(".")[0];
    animation_data[id] = {"data" => result, "processed" => false}
    output_path = filename.gsub(%r{^#{INPUT_PATH}}, OUTPUT_PATH);
    FileUtils.mkpath File.dirname(output_path)
end

animation_data.keys.each do |id|
    retrieveAllImageDependencies(animation_data, id)
    
    # Save
    outputFilename = OUTPUT_PATH + "/" + id + ".json"
    File.open(outputFilename, 'w') do |f|
       f.write(animation_data[id].to_json)
    end
end

File.open(OUTPUT_PATH + "animations.json", 'w') do |f|
   f.write(animation_data.keys.to_json)
end


