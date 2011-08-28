$.getJSON('../../static/assets/animations/animations.json', function(data) {
  $.each(data, function(val) {
    $('#animationSelect').append($('<option></option>').val(val).html(data[val]));
  });
});

var onPlayButtonClicked = function() {
    var filename = $('#animationSelect option:selected').text();
    playAnimation('../../static/assets/animations/' + filename + ".json");
}