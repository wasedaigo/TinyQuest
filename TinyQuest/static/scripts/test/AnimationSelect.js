$.getJSON('../../static/assets/animations/animations.json', function(data) {
  /*$.each(data, function(val) {
    $('#animationSelect').append($('<option></option>').val(val).html(data[val]));
  });*/
  
   $('#animationSelect').append($('<option></option>').val("../../static/assets/animations/Battle/Skills/Effect/SmokeRing").html("Battle/Skills/Effect/SmokeRing"));
});

var onPlayButtonClicked = function() {
    var id = $('#animationSelect option:selected').text();
    playAnimation(id);
}