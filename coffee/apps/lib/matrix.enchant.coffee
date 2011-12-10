enchant.matrix = {}
enchant.matrix.createMatrixIdentity = (n) ->
  matrix = new Array(n)
  i = 0

  while i < n
    matrix[i] = new Array(n)
    i++
  i = 0
  while i < n
    j = 0
    while j < n
      matrix[i][j] = (if (i is j) then 1.0 else 0.0)
      j++
    i++
  matrix

enchant.matrix.createInverseMatrix = (srcMatrix, n) ->
  matrix = Utils.clone(srcMatrix)
  invertMatrix = enchant.matrix.createMatrixIdentity(n)
  buf = 0
  i = undefined
  j = undefined
  k = 0
  i = 0
  while i < n
    buf = 1 / matrix[i][i]
    j = 0
    while j < n
      matrix[i][j] *= buf
      invertMatrix[i][j] *= buf
      j++
    j = 0
    while j < n
      unless i is j
        buf = matrix[j][i]
        k = 0
        while k < n
          matrix[j][k] -= matrix[i][k] * buf
          invertMatrix[j][k] -= invertMatrix[i][k] * buf
          k++
      j++
    i++
  invertMatrix

enchant.matrix.getNodeTransformMatirx = (dx, dy, rotation, scaleX, scaleY) ->
  [c, s] = [Math.cos(rotation), Math.sin(rotation)]
  [[ c * scaleX, s * scaleX, 0 ], [ -s * scaleY, c * scaleY, 0 ], [ dx, dy, 1 ]]

enchant.matrix.getImageTransformMatirx = (dx, dy, rotation, scaleX, scaleY) ->
    [c, s] = [Math.cos(rotation), Math.sin(rotation)]
    [
        [ c * scaleX, s * scaleX, 0 ], 
        [ -s * scaleY, c * scaleY, 0 ], 
        [(c * dx * scaleX - s * dy * scaleY) - dx, (s * dx * scaleX + c * dy * scaleY) - dy, 1 ]
    ]

enchant.matrix.transformPoint = (point, matrix) ->
  newPoint = new Array(2)
  newPoint[0] = point[0] * matrix[0][0] + point[1] * matrix[1][0] + matrix[2][0]
  newPoint[1] = point[0] * matrix[0][1] + point[1] * matrix[1][1] + matrix[2][1]
  newPoint

enchant.matrix.matrixMultiply = (m1, m2) ->
  result = enchant.matrix.createMatrixIdentity(3)
  x = 0

  while x < 3
    y = 0

    while y < 3
      sum = 0
      z = 0

      while z < 3
        sum += m1[x][z] * m2[z][y]
        z++
      result[x][y] = sum
      y++
    x++
  result