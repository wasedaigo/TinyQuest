$.getJSON('../../static/assets/animations/animations.json', function(data) {
//  $.each(data, function(val) {

    $('#animationSelect').append($('<option></option>').val(0).html("Battle/Skills/Sword/Test"));
//    $('#animationSelect').append($('<option></option>').val(val).html(data[val]));
//  });
});

var onPlayButtonClicked = function() {
    var id = $('#animationSelect option:selected').text();
    playAnimation(id);
}