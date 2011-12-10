AssertException = (message) ->
  @message = message
  
assert = (exp, message) ->
  throw new AssertException(message)  unless exp

AssertException::toString = ->
  "AssertException: " + @message