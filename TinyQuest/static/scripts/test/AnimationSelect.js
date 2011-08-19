$.getJSON('../../static/assets/animation/animation_list.json', function(data) {
  $.each(data, function(val) {
    $('#animationSelect').append(jQuery('<option value="' + val + '">' + data[val] + '</option>'));
    onPlayButtonClicked();
  });
});

var onPlayButtonClicked = function() {
    var filename = $('#animationSelect option:selected').text();
    $.getJSON('../../static/assets/animation/' + filename, function(data) {
        enchant.animation.Animation.LoadAnimation(data);
    });
}