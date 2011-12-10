window.game = new enchant.Game(640, 480)
beforeEach ->
  @addMatchers toBePlaying: (expectedSong) ->
    player = @actual
    player.currentlyPlayingSong is expectedSong and player.isPlaying