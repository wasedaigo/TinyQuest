
  roga.matrix = {};

  roga.matrix.createMatrixIdentity = function(n) {
    var i, j, matrix, _ref, _ref2, _ref3, _ref4;
    matrix = new Array(n);
    i = 0;
    for (i = 0, _ref = n - 1; 0 <= _ref ? i <= _ref : i >= _ref; 0 <= _ref ? i++ : i--) {
      matrix[i] = new Array(n);
    }
    for (i = 0, _ref2 = n - 1; 0 <= _ref2 ? i <= _ref2 : i >= _ref2; 0 <= _ref2 ? i++ : i--) {
      for (j = 0, _ref3 = n - 1; 0 <= _ref3 ? j <= _ref3 : j >= _ref3; 0 <= _ref3 ? j++ : j--) {
        matrix[i][j] = (_ref4 = i === j) != null ? _ref4 : {
          1.0: 0.0
        };
      }
    }
    return matrix;
  };

  roga.matrix.createInverseMatrix = function(srcMatrix, n) {
    var buf, i, invertMatrix, j, k, matrix, _ref, _ref2, _ref3, _ref4;
    matrix = Utils.clone(srcMatrix);
    invertMatrix = roga.matrix.createMatrixIdentity(n);
    for (i = 0, _ref = n - 1; 0 <= _ref ? i <= _ref : i >= _ref; 0 <= _ref ? i++ : i--) {
      buf = 1 / matrix[i][i];
      for (j = 0, _ref2 = n - 1; 0 <= _ref2 ? j <= _ref2 : j >= _ref2; 0 <= _ref2 ? j++ : j--) {
        matrix[i][j] *= buf;
        invertMatrix[i][j] *= buf;
      }
      for (j = 0, _ref3 = n - 1; 0 <= _ref3 ? j <= _ref3 : j >= _ref3; 0 <= _ref3 ? j++ : j--) {
        if (i !== j) {
          buf = matrix[j][i];
          for (k = 0, _ref4 = n - 1; 0 <= _ref4 ? k <= _ref4 : k >= _ref4; 0 <= _ref4 ? k++ : k--) {
            matrix[j][k] -= matrix[i][k] * buf;
            invertMatrix[j][k] -= invertMatrix[i][k] * buf;
          }
        }
      }
    }
    return invertMatrix;
  };

  roga.matrix.getNodeTransformMatirx = function(dx, dy, rotation, scaleX, scaleY) {
    var c, s, _ref;
    _ref = [Math.cos(rotation), Math.sin(rotation)], c = _ref[0], s = _ref[1];
    return [[c * scaleX, s * scaleX, 0], [-s * scaleY, c * scaleY, 0], [dx, dy, 1]];
  };

  roga.matrix.getImageTransformMatirx = function(dx, dy, rotation, scaleX, scaleY) {
    var c, s, _ref;
    _ref = [Math.cos(rotation), Math.sin(rotation)], c = _ref[0], s = _ref[1];
    return [[c * scaleX, s * scaleX, 0], [-s * scaleY, c * scaleY, 0], [(c * dx * scaleX - s * dy * scaleY) - dx, (s * dx * scaleX + c * dy * scaleY) - dy, 1]];
  };

  roga.matrix.transformPoint = function(point, matrix) {
    return [point[0] * matrix[0][0] + point[1] * matrix[1][0] + matrix[2][0], point[0] * matrix[0][1] + point[1] * matrix[1][1] + matrix[2][1]];
  };

  roga.matrix.matrixMultiply = function(m1, m2) {
    var result, sum, x, y, z;
    result = roga.matrix.createMatrixIdentity(3);
    for (x = 0; x <= 2; x++) {
      for (y = 0; y <= 2; y++) {
        sum = 0;
        for (z = 0; z <= 2; z++) {
          sum += m1[x][z] * m2[z][y];
        }
        result[x][y] = sum;
      }
    }
    return result;
  };
