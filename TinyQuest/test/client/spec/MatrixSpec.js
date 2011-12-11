
  describe("Matrix", function() {
    describe("createMatrixIdentity", function() {
      return it("Return value is correct", function() {
        var matrix;
        matrix = roga.matrix.createMatrixIdentity(4);
        expect(testRound(matrix[0][0])).toEqual(1);
        expect(testRound(matrix[0][1])).toEqual(0);
        expect(testRound(matrix[0][2])).toEqual(0);
        expect(testRound(matrix[0][3])).toEqual(0);
        expect(testRound(matrix[1][0])).toEqual(0);
        expect(testRound(matrix[1][1])).toEqual(1);
        expect(testRound(matrix[1][2])).toEqual(0);
        expect(testRound(matrix[1][3])).toEqual(0);
        expect(testRound(matrix[2][0])).toEqual(0);
        expect(testRound(matrix[2][1])).toEqual(0);
        expect(testRound(matrix[2][2])).toEqual(1);
        expect(testRound(matrix[2][3])).toEqual(0);
        expect(testRound(matrix[3][0])).toEqual(0);
        expect(testRound(matrix[3][1])).toEqual(0);
        expect(testRound(matrix[3][2])).toEqual(0);
        return expect(testRound(matrix[3][3])).toEqual(1);
      });
    });
    describe("transformPoint", function() {
      return it("Return value is correct", function() {
        var matrix, point;
        matrix = [[0, 1, 0], [-1, 0, 1], [3, 4, 1]];
        point = roga.matrix.transformPoint([1, 0], matrix);
        expect(point[0]).toEqual(3);
        return expect(point[1]).toEqual(5);
      });
    });
    describe("createInverseMatrix", function() {
      return it("Return value is correct", function() {
        var invertMatrix, matrix;
        matrix = [[1, 2, 0, -1], [-1, 1, 2, 0], [2, 0, 1, 1], [1, -2, -1, 1]];
        invertMatrix = roga.matrix.createInverseMatrix(matrix, 4);
        expect(testRound(invertMatrix[0][0])).toEqual(2.0);
        expect(testRound(invertMatrix[0][1])).toEqual(2.0);
        expect(testRound(invertMatrix[0][2])).toEqual(-1.0);
        expect(testRound(invertMatrix[0][3])).toEqual(3.0);
        expect(testRound(invertMatrix[1][0])).toEqual(-4.0);
        expect(testRound(invertMatrix[1][1])).toEqual(-5.0);
        expect(testRound(invertMatrix[1][2])).toEqual(3.0);
        expect(testRound(invertMatrix[1][3])).toEqual(-7.0);
        expect(testRound(invertMatrix[2][0])).toEqual(3.0);
        expect(testRound(invertMatrix[2][1])).toEqual(4.0);
        expect(testRound(invertMatrix[2][2])).toEqual(-2.0);
        expect(testRound(invertMatrix[2][3])).toEqual(5.0);
        expect(testRound(invertMatrix[3][0])).toEqual(-7.0);
        expect(testRound(invertMatrix[3][1])).toEqual(-8.0);
        expect(testRound(invertMatrix[3][2])).toEqual(5.0);
        return expect(testRound(invertMatrix[3][3])).toEqual(-11.0);
      });
    });
    return describe("matrixMultiply", function() {
      return it("Return value is correct", function() {
        var matrix1, matrix2, matrixAnswer, resultMatrix, x, y, _results;
        matrix1 = [[1, 2, 0], [0, 1, 0], [0, 1, 1]];
        matrix2 = [[1, 1, 0], [0, 0, 1], [1, 1, 0]];
        matrixAnswer = [[1, 1, 2], [0, 0, 1], [1, 1, 1]];
        resultMatrix = roga.matrix.matrixMultiply(matrix1, matrix2);
        _results = [];
        for (x = 0; x <= 2; x++) {
          _results.push((function() {
            var _results2;
            _results2 = [];
            for (y = 0; y <= 2; y++) {
              _results2.push(expect(testRound(resultMatrix[x][y])).toEqual(matrixAnswer[x][y]));
            }
            return _results2;
          })());
        }
        return _results;
      });
    });
  });
