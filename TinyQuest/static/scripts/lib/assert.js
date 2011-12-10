(function() {
  var AssertException, assert;

  AssertException = function(message) {
    return this.message = message;
  };

  window.assert = function(exp, message) {
    if (!exp) throw new AssertException(message);
  };

  AssertException.prototype.toString = function() {
    return "AssertException: " + this.message;
  };

}).call(this);
