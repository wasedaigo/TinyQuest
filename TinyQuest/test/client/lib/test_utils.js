// In order to compare two values, we need to round them to ignore floating number errors
function testRound(value) {
	if (typeof(value) == "object") {
		for (var i in value) {
			value[i] *= 100;
			value[i] = Math.round(value[i]);
			value[i] /= 100;
		}

	} else {
		value *= 100;
		value = Math.round(value);
		value /= 100;
	}
	return value;
}
