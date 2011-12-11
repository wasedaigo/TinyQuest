roga.matrix = {}
roga.matrix.createMatrixIdentity = (n) ->
  matrix = new Array(n)
  i = 0

  for i in [0..n-1]
    matrix[i] = new Array(n)

  for i in [0..n-1]
    for j in [0..n-1]
      matrix[i][j] = (i is j) ? 1.0 : 0.0

  matrix

roga.matrix.createInverseMatrix = (srcMatrix, n) ->
  matrix = Utils.clone(srcMatrix)
  invertMatrix = roga.matrix.createMatrixIdentity(n)

  for i in [0..n-1]
    buf = 1 / matrix[i][i]

    for j in [0..n-1]
      matrix[i][j] *= buf
      invertMatrix[i][j] *= buf

    for j in [0..n-1]
      unless i is j
        buf = matrix[j][i]

        for k in [0..n-1]
          matrix[j][k] -= matrix[i][k] * buf
          invertMatrix[j][k] -= invertMatrix[i][k] * buf

  invertMatrix

roga.matrix.getNodeTransformMatirx = (dx, dy, rotation, scaleX, scaleY) ->
  [c, s] = [Math.cos(rotation), Math.sin(rotation)]
  [[ c * scaleX, s * scaleX, 0 ], [ -s * scaleY, c * scaleY, 0 ], [ dx, dy, 1 ]]

roga.matrix.getImageTransformMatirx = (dx, dy, rotation, scaleX, scaleY) ->
    [c, s] = [Math.cos(rotation), Math.sin(rotation)]
    [
        [ c * scaleX, s * scaleX, 0 ], 
        [ -s * scaleY, c * scaleY, 0 ], 
        [(c * dx * scaleX - s * dy * scaleY) - dx, (s * dx * scaleX + c * dy * scaleY) - dy, 1 ]
    ]

roga.matrix.transformPoint = (point, matrix) ->
  [
    point[0] * matrix[0][0] + point[1] * matrix[1][0] + matrix[2][0],
    point[0] * matrix[0][1] + point[1] * matrix[1][1] + matrix[2][1]
  ]

roga.matrix.matrixMultiply = (m1, m2) ->
  result = roga.matrix.createMatrixIdentity(3)

  for x in [0..2]
    for y in [0..2]
      sum = 0
      for z in [0..2]
        sum += m1[x][z] * m2[z][y]
      result[x][y] = sum

  result