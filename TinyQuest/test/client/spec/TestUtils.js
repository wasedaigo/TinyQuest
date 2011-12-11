
  window.testRound = function(value) {
    var i;
    if (typeof value === "object") {
      for (i in value) {
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
  };
