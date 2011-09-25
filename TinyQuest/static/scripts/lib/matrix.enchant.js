enchant.matrix = {};

// Static function
enchant.matrix.createMatrixIdentity = function(n) {
    var matrix = new Array(n);
    for (var i = 0; i < n; i++) {
        matrix[i] = new Array(n);
    }
    for(i=0; i < n;i++){
      for(j = 0;j < n;j++){
        matrix[i][j] = (i==j) ? 1.0 : 0.0;
      }
    }
    
  return matrix;
}

// Calculate inverse matrix
enchant.matrix.createInverseMatrix = function(srcMatrix, n) {
    var matrix = Utils.clone(srcMatrix);
    var invertMatrix = enchant.matrix.createMatrixIdentity(n);
    var buf = 0; // Store temporary data
    var i, j, k = 0; // Counters

    // Gauss–Jordan elimination
    for(i = 0; i < n; i++){
      buf = 1 / matrix[i][i];
      for(j = 0; j < n; j++){
        matrix[i][j] *= buf;
        invertMatrix[i][j] *= buf;
      }
      for(j = 0;j < n; j++){
        if(i != j){
          buf = matrix[j][i];
          for(k=0; k<n; k++){
            matrix[j][k] -= matrix[i][k]*buf;
            invertMatrix[j][k] -= invertMatrix[i][k] * buf;
          }
        }
      }
    }

    return invertMatrix;
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

// Multiply 3x3 matrix to 2 point
enchant.matrix.transformPoint = function(point, matrix) {
    var newPoint = new Array(2);
    newPoint[0] = point[0] * matrix[0][0] + point[1] * matrix[1][0] + matrix[2][0];
    newPoint[1] = point[0] * matrix[0][1] + point[1] * matrix[1][1] + matrix[2][1];
    return newPoint;
}

// Multiply 3x3 matrix by 3x3 matrix
enchant.matrix.matrixMultiply = function (m1, m2) {
    var result = enchant.matrix.createMatrixIdentity(3);
    
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