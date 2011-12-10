describe "Matrix", ->
  surface = new enchant.Surface(640, 480)
  describe "createMatrixIdentity", ->
    it "Return value is correct", ->
      matrix = enchant.matrix.createMatrixIdentity(4)
      expect(testRound(matrix[0][0])).toEqual 1
      expect(testRound(matrix[0][1])).toEqual 0
      expect(testRound(matrix[0][2])).toEqual 0
      expect(testRound(matrix[0][3])).toEqual 0
      expect(testRound(matrix[1][0])).toEqual 0
      expect(testRound(matrix[1][1])).toEqual 1
      expect(testRound(matrix[1][2])).toEqual 0
      expect(testRound(matrix[1][3])).toEqual 0
      expect(testRound(matrix[2][0])).toEqual 0
      expect(testRound(matrix[2][1])).toEqual 0
      expect(testRound(matrix[2][2])).toEqual 1
      expect(testRound(matrix[2][3])).toEqual 0
      expect(testRound(matrix[3][0])).toEqual 0
      expect(testRound(matrix[3][1])).toEqual 0
      expect(testRound(matrix[3][2])).toEqual 0
      expect(testRound(matrix[3][3])).toEqual 1

  describe "transformPoint", ->
    it "Return value is correct", ->
      matrix = [ [ 0, 1, 0 ], [ -1, 0, 1 ], [ 3, 4, 1 ] ]
      point = enchant.matrix.transformPoint([ 1, 0 ], matrix)
      expect(point[0]).toEqual 3
      expect(point[1]).toEqual 5

  describe "createInverseMatrix", ->
    it "Return value is correct", ->
      matrix = [ [ 1, 2, 0, -1 ], [ -1, 1, 2, 0 ], [ 2, 0, 1, 1 ], [ 1, -2, -1, 1 ] ]
      invertMatrix = enchant.matrix.createInverseMatrix(matrix, 4)
      expect(testRound(invertMatrix[0][0])).toEqual 2.0
      expect(testRound(invertMatrix[0][1])).toEqual 2.0
      expect(testRound(invertMatrix[0][2])).toEqual -1.0
      expect(testRound(invertMatrix[0][3])).toEqual 3.0
      expect(testRound(invertMatrix[1][0])).toEqual -4.0
      expect(testRound(invertMatrix[1][1])).toEqual -5.0
      expect(testRound(invertMatrix[1][2])).toEqual 3.0
      expect(testRound(invertMatrix[1][3])).toEqual -7.0
      expect(testRound(invertMatrix[2][0])).toEqual 3.0
      expect(testRound(invertMatrix[2][1])).toEqual 4.0
      expect(testRound(invertMatrix[2][2])).toEqual -2.0
      expect(testRound(invertMatrix[2][3])).toEqual 5.0
      expect(testRound(invertMatrix[3][0])).toEqual -7.0
      expect(testRound(invertMatrix[3][1])).toEqual -8.0
      expect(testRound(invertMatrix[3][2])).toEqual 5.0
      expect(testRound(invertMatrix[3][3])).toEqual -11.0