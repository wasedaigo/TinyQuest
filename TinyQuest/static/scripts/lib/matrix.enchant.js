enchant.matrix = {};

// Static function
enchant.matrix.createMatrixIdentity = function() {
  return [
    [1, 0, 0],
    [0, 1, 0],
    [0, 0, 1]
  ];
}

// Transformation 
// 1. Rotate at origin
// 2. Scale at origin
// 3. Translate to center point
enchant.matrix.getNodeTransformMatirx = function (dx, dy, rotation, scaleX, scaleY) {
  var c = Math.cos(rotation);
  var s = Math.sin(rotation);
  var result = [
                [c * scaleX, s * scaleY, 0],
                [-s * scaleX, c * scaleY, 0],
                [dx, dy, 1]
                ];
  return result;
};

// Transformation
// 1. Translate to center point
// 2. Rotate
// 3. Scale
// 4. Translate back to the original point 
enchant.matrix.getImageTransformMatirx = function (dx, dy, rotation, scaleX, scaleY) {
  var c = Math.cos(rotation);
  var s = Math.sin(rotation);
  var result = [
                [c * scaleX, s * scaleY, 0],
                [-s * scaleX, c * scaleY, 0],
                [(c * dx - s * dy) * scaleX - dx, (s * dx + c * dy) * scaleY - dy, 1]
                ];
  return result;
};

enchant.matrix.matrixMultiply = function (m1, m2) {
  var result = enchant.matrix.createMatrixIdentity();

  for (var x = 0; x < 3; x++) {
    for (var y = 0; y < 3; y++) {
      var sum = 0;

      for (var z = 0; z < 3; z++) {
        sum += m1[x][z] * m2[z][y];
      }

      result[x][y] = sum;
    }
  }
  return result;
};