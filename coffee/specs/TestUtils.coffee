window.testRound = (value) ->
  if typeof (value) is "object"
    for i of value
      value[i] *= 100
      value[i] = Math.round(value[i])
      value[i] /= 100
  else
    value *= 100
    value = Math.round(value)
    value /= 100
  value