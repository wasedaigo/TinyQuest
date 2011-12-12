###*
 * enchant.coffee v0.4.1
 *
 * Copyright (c) hanachin_
 * Dual licensed under the MIT or GPL Version 3 licenses
###
###*
 * enchant.js v0.4.1
 *
 * Copyright (c) Ubiquitous Entertainment Inc.
 * Dual licensed under the MIT or GPL Version 3 licenses
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
###

Function::define = (prop, desc) ->
  Object.defineProperty this.prototype, prop, desc

if (typeof Object.defineProperty isnt 'function')
  Object.defineProperty = (obj, prop, desc) ->
    if ('value' in desc) then obj[prop] =  desc.value
    if ('get' in desc) then obj.__defineGetter__(prop, desc.get)
    if ('set' in desc) then obj.__defineSetter__(prop, desc.set)
    obj

if (typeof Object.defineProperties isnt 'function')
  Object.defineProperties = (obj, descs) ->
    for own prop, value of descs
      Object.defineProperty(obj, prop, descs[prop])
    obj

# FIXME:ここちょっと後で直すかも
if (typeof Object.create isnt 'function')
  Object.create = (prototype, descs) ->
    class F
    F = ->
    F.prototype = prototype
    obj = new F
    if (descs?) then Object.defineProperties(obj, descs)
    obj

if (typeof Object.getPrototypeOf isnt 'function')
  Object.getPrototypeOf = (obj) ->
    obj.__proto__

###*
 * グローバルにライブラリのクラスをエクスポートする.
 *
 * 引数に何も渡さない場合enchant.jsで定義されたクラス及びプラグインで定義されたクラス
 * 全てがエクスポートされる. 引数が一つ以上の場合はenchant.jsで定義されたクラスのみ
 * がデフォルトでエクスポートされ, プラグインのクラスをエクスポートしたい場合は明示的に
 * プラグインの識別子を引数として渡す必要がある.
 *
 * @example
 *   enchant();     // 全てのクラスがエクスポートされる
 *   enchant('');   // enchant.js本体のクラスのみがエクスポートされる
 *   enchant('ui'); // enchant.js本体のクラスとui.enchant.jsのクラスがエクスポートされる
 *
 * @param {...String} [modules] エクスポートするモジュール. 複数指定できる.
###
enchant = (modules) ->
  if (modules?)
    if (modules not instanceof Array)
      modules = Array.prototype.slice.call(arguments)
    modules = modules.filter (module) -> [module].join()
  ((m, p) ->
    include = (module, prefix) ->
      submodules = []
      for own prop, value of module
        if (typeof module[prop] is 'function')
          window[prop] = module[prop]
        else if (Object.getPrototypeOf(module[prop]) is Object.prototype)
          if (modules?)
            i = modules.indexOf(prefix + prop)
            if (i isnt -1)
              submodules.push(prop)
              modules.splice(i, 1)
          else
            submodules.push(prop)
      for sub in submodules
        include(module[sub], "#{prefix}#{sub}.")
    include(m, p)
  )(enchant, '')
  if (modules? and modules.length)
    throw new Error "Cannot load module: #{modules.join(', ')}"

(->
  "use strict"

  VENDER_PREFIX = (->
    ua = navigator.userAgent
    if (ua.indexOf("Opera") isnt -1) then "0"
    else if (ua.indexOf("MSIE") isnt -1) then "ms"
    else if (ua.indexOf("WebKit") isnt -1) then "webkit"
    else if (navigator.product is "Gecko") then "Moz"
    else ""
  )()

  TOUCH_ENABLED = (->
    div = document.createElement('div')
    div.setAttribute('ontouchstart', 'return')
    typeof div.ontouchstart is 'function'
  )()

  RETINA_DISPLAY = (->
    if (navigator.userAgent.indexOf('iPhone') isnt -1 and window.devicePixelRatio is 2)
      viewport = document.querySelector('meta[name="viewport"]')
      if (viewport?)
        viewport = document.createElement('meta')
        document.head.appendChild(viewport)
      viewport.setAttribute('content', 'width=640px')
      true
    else false
  )()

  # the running instance
  game = null

  ###*
   * @scope enchant.Event.prototype
  ###
  class enchant.Event
    ###*
    * DOM Event風味の独自イベント実装を行ったクラス.
    * ただしフェーズの概念はなし.
    * @param {String} type Eventのタイプ
    * @constructs
    ###
    constructor: (type) ->
      ###*
      * イベントのタイプ.
      * @type {String}
      ###
      @type = type
      ###*
      * イベントのターゲット.
      * @type {*}
      ###
      @target = null
      ###*
      * イベント発生位置のx座標.
      * @type {Number}
      ###
      @x = 0
      ###*
      * イベント発生位置のy座標.
      * @type {Number}
      ###
      @y = 0
      ###*
      * イベントを発行したオブジェクトを基準とするイベント発生位置のx座標.
      * @type {Number}
      ###
      @localX = 0
      ###*
      * イベントを発行したオブジェクトを基準とするイベント発生位置のy座標.
      * @type {Number}
      ###
      @localY = 0
    _initPosition: (pageX, pageY) ->
      @x = @localX = (pageX - game._pageX) / game.scale
      @y = @localY = (pageY - game._pageY) / game.scale

  ###*
   * Gameのロード完了時に発生するイベント.
   *
   * 画像のプリロードを行う場合ロードが完了するのを待ってゲーム開始時の処理を行う必要がある.
   * 発行するオブジェクト: enchant.Game
   *
   * @example
   *   var game = new Game(320, 320);
   *   game.preload('player.gif');
   *   game.onload = function() {
   *      ... // ゲーム開始時の処理を記述
   *   };
   *   game.start();
   *
   * @type {String}
  ###
  enchant.Event.LOAD = 'load'

  ###*
   * Gameのロード進行中に発生するイベント.
   * プリロードする画像が一枚ロードされる度に発行される. 発行するオブジェクト: enchant.Game
   * @type {String}
  ###
  enchant.Event.PROGRESS = 'progress'

  ###*
   * フレーム開始時に発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Node
   * @type {String}
  ###
  enchant.Event.ENTER_FRAME = 'enterframe'

  ###*
   * フレーム終了時に発生するイベント.
   * 発行するオブジェクト: enchant.Game
   * @type {String}
  ###
  enchant.Event.EXIT_FRAME = 'exitframe'

  ###*
   * Sceneが開始したとき発生するイベント.
   * 発行するオブジェクト: enchant.Scene
   * @type {String}
  ###
  enchant.Event.ENTER = 'enter'

  ###*
   * Sceneが終了したとき発生するイベント.
   * 発行するオブジェクト: enchant.Scene
   * @type {String}
  ###
  enchant.Event.EXIT = 'exit'

  ###*
   * NodeがGroupに追加されたとき発生するイベント.
   * 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.ADDED = 'added'

  ###*
   * NodeがSceneに追加されたとき発生するイベント.
   * 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.ADDED_TO_SCENE = 'addedtoscene'

  ###*
   * NodeがGroupから削除されたとき発生するイベント.
   * 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.REMOVED = 'removed'

  ###*
   * NodeがSceneから削除されたとき発生するイベント.
   * 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.REMOVED_FROM_SCENE = 'removedfromscene'

  ###*
   * Nodeに対するタッチが始まったとき発生するイベント.
   * クリックもタッチとして扱われる. 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.TOUCH_START = 'touchstart'

  ###*
   * Nodeに対するタッチが移動したとき発生するイベント.
   * クリックもタッチとして扱われる. 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.TOUCH_MOVE = 'touchmove'

  ###*
   * Nodeに対するタッチが終了したとき発生するイベント.
   * クリックもタッチとして扱われる. 発行するオブジェクト: enchant.Node
   * @type {String}
  ###
  enchant.Event.TOUCH_END = 'touchend'

  ###*
   * Entityがレンダリングされるときに発生するイベント.
   * 発行するオブジェクト: enchant.Entity
   * @type {String}
  ###
  enchant.Event.RENDER = 'render'

  ###*
   * ボタン入力が始まったとき発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.INPUT_START = 'inputstart'

  ###*
   * ボタン入力が変化したとき発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.INPUT_CHANGE = 'inputchange'

  ###*
   * ボタン入力が終了したとき発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.INPUT_END = 'inputend'

  ###*
   * leftボタンが押された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.LEFT_BUTTON_DOWN = 'leftbuttondown'

  ###*
   * leftボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.LEFT_BUTTON_UP = 'leftbuttonup'

  ###*
   * rightボタンが押された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.RIGHT_BUTTON_DOWN = 'rightbuttondown'

  ###*
   * rightボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.RIGHT_BUTTON_UP = 'rightbuttonup'

  ###*
   * upボタンが押された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.UP_BUTTON_DOWN = 'upbuttondown'

  ###*
   * upボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.UP_BUTTON_UP = 'upbuttonup'

  ###*
   * downボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.DOWN_BUTTON_DOWN = 'downbuttondown'

  ###*
   * downボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.DOWN_BUTTON_UP = 'downbuttonup'

  ###*
   * aボタンが押された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.A_BUTTON_DOWN = 'abuttondown'

  ###*
   * aボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.A_BUTTON_UP = 'abuttonup'

  ###*
   * bボタンが押された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.B_BUTTON_DOWN = 'bbuttondown'

  ###*
   * bボタンが離された発生するイベント.
   * 発行するオブジェクト: enchant.Game, enchant.Scene
   * @type {String}
  ###
  enchant.Event.B_BUTTON_UP = 'bbuttonup'

  ###*
   * @scope enchant.EventTarget.prototype
  ###
  class enchant.EventTarget
    ###*
    * DOM Event風味の独自イベント実装を行ったクラス.
    * ただしフェーズの概念はなし.
    * @constructs
    ###
    constructor: ->
      @_listeners = {}
      # Eventクラスで定義されたイベント名と
      # 同じ名前のメソッドを持っていれば呼び出すようにする
      for own prop, ev of enchant.Event
        @addEventListener(ev, ((ev) -> ((e) -> @[ev](e) if @[ev]?))(ev))

    ###*
    * イベントリスナを追加する.
    * @param {String} type イベントのタイプ.
    * @param {function(e:enchant.Event)} listener 追加するイベントリスナ.
    ###
    addEventListener: (type, listener) ->
      listeners = @_listeners[type]
      if not listeners? then @_listeners[type] = [listener]
      else if (listeners.indexOf(listener) is -1) then listeners.unshift(listener)

    ###*
    * イベントリスナを削除する.
    * @param {String} type イベントのタイプ.
    * @param {function(e:enchant.Event)} listener 削除するイベントリスナ.
    ###
    removeEventListener: (type, listener) ->
      listeners = @_listeners[type]
      if (listeners?)
        i = listeners.indexOf(listener)
        if (i isnt -1) then listeners.splice(i, 1)

    ###*
    * イベントを発行する.
    * @param {enchant.Event} e 発行するイベント.
    ###
    dispatchEvent: (e) ->
      e.target = this
      e.localX = e.x - @_offsetX
      e.localY = e.y - @_offsetY
      if (this['on' + e.type]?) then this['on' + e.type]()
      listeners = @_listeners[e.type]
      if (listeners?)
        listeners = listeners.slice()
        listener.call(this, e) for listener in listeners

  ###*
   * @scope enchant.Game.prototype
  ###
  class enchant.Game extends enchant.EventTarget
    ###*
    * ゲームのメインループ, シーンを管理するクラス.
    *
    * インスタンスは一つしか存在することができず, すでにインスタンスが存在する状態で
    * コンストラクタを実行した場合既存のものが上書きされる. 存在するインスタンスには
    * enchant.Game.instanceからアクセスできる.
    *
    * @param {Number} width ゲーム画面の横幅.
    * @param {Number} height ゲーム画面の高さ.
    * @constructs
    * @extends enchant.EventTarget
    ###
    constructor: (width, height) ->
      super
      initial = true
      if (game)
        initial = false
        game.stop()
      game = enchant.Game.instance = this
      ###*
      * ゲーム画面の横幅.
      * @type {Number}
      ###
      @width = width or 320
      ###*
      * ゲーム画面の高さ.
      * @type {Number}
      ###
      @height = height or 320
      ###*
      * ゲームの表示倍率.
      * @type {Number}
      ###
      @scale = 1
      stage = document.getElementById('enchant-stage')
      if (not stage)
        stage = document.createElement('div')
        stage.id = 'enchant-stage'
        stage.style.width = "#{window.innerWidth}px"
        stage.style.height = "#{window.innerHeight}px"
        stage.style.position = 'absolute'
        if (document.body.firstChild)
          document.body.insertBefore(stage, document.body.firstChild)
        else
          document.body.appendChild(stage)
        @scale = Math.min(
          window.innerWidth / @width,
          window.innerHeight / @height
        )
        @_pageX = 0
        @_pageY = 0
      else
        style = window.getComputedStyle(stage)
        width = parseInt(style.width)
        height = parseInt(style.height)
        if (width and height)
          @scale = Math.min(
            width / @width,
            height / @height
          )
        else
          stage.style.width = "#{@width}px"
          stage.style.height = "#{@height}px"
        while (stage.firstChild)
          stage.removeChild(stage.firstChild)
        stage.style.position = 'relative'
        bounding = stage.getBoundingClientRect()
        @_pageX = Math.round(window.scrollX + bounding.left)
        @_pageY = Math.round(window.scrollY + bounding.top)

      @scale ||= 1
      stage.style.fontSize = '12px'
      stage.style.webkitTextSizeAdjust = 'none'
      @_element = stage

      ###*
      * ゲームのフレームレート.
      * @type {Number}
      ###
      @fps = 30
      ###*
      * ゲーム開始からのフレーム数.
      * @type {Number}
      ###
      @frame = 0
      ###*
      * ゲームが実行可能な状態かどうか.
      * @type {Boolean}
      ###
      @ready = null
      ###*
      * ゲームが実行状態かどうか.
      * @type {Boolean}
      ###
      @running = false
      ###*
      * ロードされた画像をパスをキーとして保存するオブジェクト.
      * @type {Object.<String, Surface>}
      ###
      @assets = {}
      assets = @_assets = []
      ((m) ->
        detectAssets = (module) ->
          if (module.assets instanceof Array)
            [].push.apply(assets, module.assets)
          for own prop, value of module
            if (Object.getPrototypeOf(module[prop]) is Object.prototype)
              detectAssets(module[prop])
        detectAssets(m)
      )(enchant)

      @_scenes = []
      ###*
      * 現在のScene. Sceneスタック中の一番上のScene.
      * @type {enchant.Scene}
      ###
      @currentScene = null
      ###*
      * ルートScene. Sceneスタック中の一番下のScene.
      * @type {enchant.Scene}
      ###
      @rootScene = new enchant.Scene
      @pushScene(@rootScene)
      ###*
      * ローディング時に表示されるScene.
      * @type {enchant.Scene}
      ###
      @loadingScene = new enchant.Scene()
      @loadingScene.backgroundColor = '#000'
      barWidth = @width * 0.9 | 0
      barHeight = @width * 0.3 | 0
      border = barWidth * 0.05 | 0
      bar = new enchant.Sprite(barWidth, barHeight)
      bar.x = (@width - barWidth) / 2
      bar.y = (@height - barHeight) / 2
      image = new enchant.Surface(barWidth, barHeight)
      image.context.fillStyle = '#fff'
      image.context.fillRect(0, 0, barWidth, barHeight)
      image.context.fillStyle = '#000'
      image.context.fillRect(border, border, barWidth - border*2, barHeight - border*2)
      bar.image = image
      progress = 0
      _progress = 0
      @addEventListener('progress', (e) ->
        progress = e.loaded / e.total
      )
      bar.addEventListener('enterframe', ->
        _progress *= 0.9
        _progress += progress * 0.1
        image.context.fillStyle = '#fff'
        image.context.fillRect(border, 0, (barWidth - border*2) * _progress, barHeight)
      )
      @loadingScene.addChild(bar)

      @_mousedownID = 0
      @_surfaceID = 0
      @_soundID = 0
      @_intervalID = null

      ###*
      * ゲームに対する入力状態を保存するオブジェクト.
      * @type {Object.<String, Boolean>}
      ###
      @input = {}
      @_keybind = {}
      @keybind(37, 'left')  # Left Arrow
      @keybind(38, 'up')    # Up Arrow
      @keybind(39, 'right') # Right Arrow
      @keybind(40, 'down')  # Down Arrow
      c = 0
      ['left', 'right', 'up', 'down', 'a', 'b'].forEach((type) ->
        @addEventListener("#{type}buttondown", (e) ->
          if (not @input[type])
            @input[type] = true
            @dispatchEvent(new enchant.Event(if c++ then 'inputchange' else 'inputstart'))
          @currentScene.dispatchEvent(e)
        )
        @addEventListener("#{type}buttonup", (e) ->
          if (@input[type])
            @input[type] = false
            @dispatchEvent(new enchant.Event(if --c then 'inputchange' else 'inputend'))
          @currentScene.dispatchEvent(e)
        )
      , this)

      if (initial)
        document.addEventListener('keydown', (e) ->
          game.dispatchEvent(new enchant.Event('keydown'))
          if ((37 <= e.keyCode and e.keyCode <= 40) or e.keyCode is 32)
            e.preventDefault()
            e.stopPropagation()

          if not game.running then return

          button = game._keybind[e.keyCode]
          if (button)
            e = new enchant.Event("#{button}buttondown")
            game.dispatchEvent(e)
        , true)
        document.addEventListener('keyup', (e) ->
          if not game.running then return
          button = game._keybind[e.keyCode]
          if (button)
            e = new enchant.Event("#{button}buttonup")
            game.dispatchEvent(e)
        , true)
        if (TOUCH_ENABLED)
          document.addEventListener('touchstart', (e) ->
            e.preventDefault()
          , true)
          document.addEventListener('touchmove', (e) ->
            e.preventDefault()
            if not game.running then e.stopPropagation()
          , true)
          document.addEventListener('touchend', (e) ->
            e.preventDefault()
            if not game.running then e.stopPropagation()
          , true)
        else
          document.addEventListener('mousedown', (e) ->
            e.preventDefault()
            game._mousedownID++
            if not game.running then e.stopPropagation()
          , true)
          document.addEventListener('mousemove', (e) ->
            e.preventDefault()
            if not game.running then e.stopPropagation()
          , true)
          document.addEventListener('mouseup', (e) ->
            e.preventDefault()
            if not game.running then e.stopPropagation()
          , true)
    ###*
    * ファイルのプリロードを行う.
    *
    * プリロードを行うよう設定されたファイルはenchant.Game#startが実行されるとき
    * ロードが行われる. 全てのファイルのロードが完了したときはGameオブジェクトからload
    * イベントが発行され, Gameオブジェクトのassetsプロパティから画像ファイルの場合は
    * Surfaceオブジェクトとして, 音声ファイルの場合はSoundオブジェクトとして,
    * その他の場合は文字列としてアクセスできるようになる.
    *
    * なおこのSurfaceオブジェクトはenchant.Surface.loadを使って作成されたものである
    * ため直接画像操作を行うことはできない. enchant.Surface.loadの項を参照.
    *
    * @example
    *   game.preload('player.gif');
    *   game.onload = function() {
    *      var sprite = new Sprite(32, 32);
    *      sprite.image = game.assets['player.gif']; // パス名でアクセス
    *      ...
    *   };
    *   game.start();
    *
    * @param {...String} assets プリロードする画像のパス. 複数指定できる.
    ###
    preload: (assets) ->
      if assets not instanceof Array
        assets = Array.prototype.slice.call(arguments)
      [].push.apply(@_assets, assets)
    ###*
    * ファイルのロードを行う.
    *
    * @param {String} asset ロードするファイルのパス.
    * @param {Function} [callback] ファイルのロードが完了したときに呼び出される関数.
    ###
    load: (src, callback) ->
      # loadイベント時の呼び出しは無視
      return if src instanceof Event
      callback ?= ->

      ext = src.match(/\.\w+$/)[0]
      if (ext) then ext = ext.slice(1).toLowerCase()
      switch ext
        when 'jpg', 'gif', 'png'
          game.assets[src] = enchant.Surface.load(src)
          game.assets[src].addEventListener('load', callback)
        when 'mp3', 'aac', 'm4a', 'wav', 'ogg'
          game.assets[src] = enchant.Sound.load(src, "audio/#{ext}")
          game.assets[src].addEventListener('load', callback)
        else
          req = new XMLHttpRequest()
          req.open('GET', src, true)
          req.onreadystatechange = (e) ->
            if (req.readyState is 4)
              if (req.status isnt 200)
                throw new Error("Cannot load an asset: #{src}")

              type = req.getResponseHeader('Content-Type') || ''
              if (type.match(/^image/))
                game.assets[src] = enchant.Surface.load(src)
                game.assets[src].addEventListener('load', callback)
              else if (type.match(/^audio/))
                game.assets[src] = enchant.Sound.load(src, type)
                game.assets[src].addEventListener('load', callback)
              else
                game.assets[src] = req.responseText
                callback()
          req.send(null)
    ###*
    * ゲームを開始する.
    *
    * enchant.Game#fpsで設定されたフレームレートに従ってenchant.Game#currentSceneの
    * フレームの更新が行われるようになる. プリロードする画像が存在する場合はロードが
    * 始まりローディング画面が表示される.
    ###
    start: ->
      if (@_intervalID) then window.clearInterval(@_intervalID)
      else if (@_assets.length)
        if (enchant.Sound.enabledInMobileSafari and not game._touched and VENDER_PREFIX is 'webkit' and TOUCH_ENABLED)
          scene = new Scene
          scene.backgroundColor = '#000'
          size = Math.round(game.width / 10)
          sprite = new Sprite(game.width, size)
          sprite.y = (game.height - size) / 2
          sprite.image = new Surface(game.width, size)
          sprite.image.context.fillStyle = '#fff'
          sprite.image.context.font = "#{(size-1)}px bold Helvetica,Arial,sans-serif"
          width = sprite.image.context.measureText('Touch to Start').width
          sprite.image.context.fillText('Touch to Start', (game.width - width) / 2, size-1)
          scene.addChild(sprite)
          document.addEventListener('touchstart', ->
              game._touched = true
              game.removeScene(scene)
              game.start()
          , true)
          game.pushScene(scene)
          return

        o = {}
        assets = @_assets.filter (asset) ->
          if asset in o then false else (o[asset] = true)
        loaded = 0
        len = assets.length
        for asset in assets
          @load(asset, ->
            e = new enchant.Event('progress')
            e.loaded = ++loaded
            e.total = len
            game.dispatchEvent(e)
            if (loaded is len)
              game.removeScene(game.loadingScene)
              game.dispatchEvent(new enchant.Event('load'))
          )
        @pushScene(@loadingScene)
      else
        @dispatchEvent(new enchant.Event('load'))
      @currentTime = Date.now()
      @_intervalID = window.setInterval((-> game._tick()), 1000 / @fps)
      @running = true

    _tick: ->
      now = Date.now()
      e = new enchant.Event('enterframe')
      e.elapsed = now - @currentTime
      @currentTime = now

      nodes = @currentScene.childNodes.slice()
      push = Array.prototype.push
      while (nodes.length)
        node = nodes.pop()
        node.dispatchEvent(e)
        if (node.childNodes)
          push.apply(nodes, node.childNodes)

      @currentScene.dispatchEvent(e)
      @dispatchEvent(e)

      @dispatchEvent(new enchant.Event('exitframe'))
      @frame++
    ###*
    * ゲームを停止する.
    *
    * フレームは更新されず, プレイヤーの入力も受け付けなくなる.
    * enchant.Game#startで再開できる.
    ###
    stop: ->
      if (@_intervalID)
        window.clearInterval(@_intervalID)
        @_intervalID = null
      @running = false
    ###*
    * ゲームを一時停止する.
    *
    * フレームは更新されず, プレイヤーの入力は受け付ける.
    * enchant.Game#startで再開できる.
    ###
    pause: ->
      if (@_intervalID)
        window.clearInterval(@_intervalID)
        @_intervalID = null
    ###*
     * 新しいSceneに移行する.
     *
     * Sceneはスタック状に管理されており, 表示順序もスタックに積み上げられた順に従う.
     * enchant.Game#pushSceneを行うとSceneをスタックの一番上に積むことができる. スタックの
     * 一番上のSceneに対してはフレームの更新が行われる.
     *
     * @param {enchant.Scene} scene 移行する新しいScene.
     * @return {enchant.Scene} 新しいScene.
    ###
    pushScene: (scene) ->
      @_element.appendChild(scene._element)
      if (@currentScene)
        @currentScene.dispatchEvent(new enchant.Event('exit'))
      @currentScene = scene
      @currentScene.dispatchEvent(new enchant.Event('enter'))
      @_scenes.push(scene)
    ###*
     * 現在のSceneを終了させ前のSceneに戻る.
     *
     * Sceneはスタック状に管理されており, 表示順序もスタックに積み上げられた順に従う.
     * enchant.Game#popSceneを行うとスタックの一番上のSceneを取り出すことができる.
     *
     * @return {enchant.Scene} 終了させたScene.
    ###
    popScene: ->
      if (@currentScene is @rootScene) then  return
      @_element.removeChild(@currentScene._element)
      @currentScene.dispatchEvent(new enchant.Event('exit'))
      @currentScene = @_scenes[@_scenes.length-2]
      @currentScene.dispatchEvent(new enchant.Event('enter'))
      return @_scenes.pop()
    ###*
     * 現在のSceneを別のSceneにおきかえる.
     *
     * enchant.Game#popScene, enchant.Game#pushSceneを同時に行う.
     *
     * @param {enchant.Scene} scene おきかえるScene.
     * @return {enchant.Scene} 新しいScene.
    ###
    replaceScene: (scene) ->
      @popScene()
      @pushScene(scene)
    ###*
     * Scene削除する.
     *
     * Sceneスタック中からSceneを削除する.
     *
     * @param {enchant.Scene} scene 削除するScene.
     * @return {enchant.Scene} 削除したScene.
    ###
    removeScene: (scene) ->
      if (@currentScene is scene)
        @popScene()
      else
        i = @_scenes.indexOf(scene)
        if (i isnt -1)
          @_scenes.splice(i, 1)
          @_element.removeChild(scene._element)
          scene
    ###*
     * キーバインドを設定する.
     *
     * キー入力をleft, right, up, down, a, bいずれかのボタン入力として割り当てる.
     *
     * @param {Number} key キーバインドを設定するキーコード.
     * @param {String} button 割り当てるボタン.
    ###
    keybind: (key, button) ->
      @_keybind[key] = button

  ###*
  * 現在のGameインスタンス.
  * @type {enchant.Game}
  * @static
  ###
  enchant.Game.instance = null

  ###*
  * @scope enchant.Node.prototype
  ###
  class enchant.Node extends enchant.EventTarget
    ###*
    * Sceneをルートとした表示オブジェクトツリーに属するオブジェクトの基底クラス.
    * 直接使用することはない.
    * @constructs
    * @extends enchant.EventTarget
    ###
    constructor: ->
      super

      @_x = 0
      @_y = 0
      @_offsetX = 0
      @_offsetY = 0

      ###*
       * Nodeの親Node.
       * @type {enchant.Group}
      ###
      @parentNode = null
      ###*
      * Nodeが属しているScene.
      * @type {enchant.Scene}
      ###
      @scene = null

      @addEventListener('touchstart', (e) ->
        if (@parentNode and @parentNode isnt @scene)
          @parentNode.dispatchEvent(e)
      )
      @addEventListener('touchmove', (e) ->
        if (@parentNode and @parentNode isnt @scene)
          @parentNode.dispatchEvent(e)
      )
      @addEventListener('touchend', (e) ->
        if (@parentNode and @parentNode isnt @scene)
          @parentNode.dispatchEvent(e)
      )
    ###*
    * Nodeを移動する.
    * @param {Number} x 移動先のx座標.
    * @param {Number} y 移動先のy座標.
    ###
    moveTo: (x, y) ->
      @_x = x
      @_y = y
      @_updateCoordinate()
    ###*
    * Nodeを移動する.
    * @param {Number} x 移動するx軸方向の距離.
    * @param {Number} y 移動するy軸方向の距離.
    ###
    moveBy: (x, y) ->
      @_x += x
      @_y += y
      @_updateCoordinate()
    ###*
    * Nodeのx座標.
    * @type {Number}
    ###
    @define 'x'
      get: -> @_x
      set: (x) ->
        @_x = x
        @_updateCoordinate()
    ###*
    * Nodeのy座標.
    * @type {Number}
    ###
    @define 'y'
      get: -> @_y
      set: (y) ->
        @_y = y
        @_updateCoordinate()

    _updateCoordinate: ->
      if (@parentNode)
        @_offsetX = @parentNode._offsetX + @_x
        @_offsetY = @parentNode._offsetY + @_y
      else
        @_offsetX = @_x
        @_offsetY = @_y

  ###*
  * @scope enchant.Entity.prototype
  ###
  class enchant.Entity extends enchant.Node
    ###*
    * DOM上で表示する実体を持ったクラス.直接使用することはない.
    * @constructs
    * @extends enchant.Node
    ###
    constructor: ->
      super

      @_element = document.createElement('div')
      @_style = @_element.style
      @_style.position = 'absolute'

      @_width = 0
      @_height = 0
      @_backgroundColor = null
      @_opacity = 1
      @_visible = true
      @_buttonMode = null

      ###*
      * Entityにボタンの機能を設定する.
      * Entityに対するタッチ, クリックをleft, right, up, down, a, bいずれかの
      * ボタン入力として割り当てる.
      * @type {String}
      ###
      @buttonMode = null
      ###*
       * Entityが押されているかどうか.
       * buttonModeが設定されているときだけ機能する.
       * @type {Boolean}
      ###
      @buttonPressed = false
      @addEventListener('touchstart', ->
        if not @buttonMode then return
        @buttonPressed = true
        e = new Event("#{button}buttondown")
        @dispatchEvent(e)
        game.dispatchEvent(e)
      )
      @addEventListener('touchend', ->
        if not @buttonMode then return
        @buttonPressed = false
        e = new Event("#{button}buttonup")
        @dispatchEvent(e)
        game.dispatchEvent(e)
      )
      that = this
      render = ->
        that.dispatchEvent(new enchant.Event('render'))
      @addEventListener('addedtoscene', ->
        render()
        game.addEventListener('exitframe', render)
      )
      @addEventListener('removedfromscene', ->
        game.removeEventListener('exitframe', render)
      )
      @addEventListener('render', ->
        if (@_offsetX isnt @_previousOffsetX)
          @_style.left = "#{@_offsetX}px"
        if (@_offsetY isnt @_previousOffsetY)
          @_style.top = "#{@_offsetY}px"
        @_previousOffsetX = @_offsetX
        @_previousOffsetY = @_offsetY
      )
      that = this
      if (TOUCH_ENABLED)
        @_element.addEventListener('touchstart', (e) ->
          touches = e.touches
          for touch in touches
            e = new enchant.Event('touchstart')
            e.identifier = touch.identifier
            e._initPosition(touch.pageX, touch.pageY)
            that.dispatchEvent(e)
        , false)
        @_element.addEventListener('touchmove', (e) ->
          touches = e.touches
          for touch in touches
            e = new enchant.Event('touchmove')
            e.identifier = touch.identifier
            e._initPosition(touch.pageX, touch.pageY)
            that.dispatchEvent(e)
        , false)
        @_element.addEventListener('touchend', (e) ->
          touches = e.changedTouches
          for touch in touches
            e = new enchant.Event('touchend')
            e.identifier = touch.identifier
            e._initPosition(touch.pageX, touch.pageY)
            that.dispatchEvent(e)
        , false)
      else
        @_element.addEventListener('mousedown', (e) ->
          x = e.pageX
          y = e.pageY
          e = new enchant.Event('touchstart')
          e.identifier = game._mousedownID
          e._initPosition(x, y)
          that.dispatchEvent(e)
          that._mousedown = true
        , false)
        game._element.addEventListener('mousemove', (e) ->
          if not that._mousedown then return
          x = e.pageX
          y = e.pageY
          e = new enchant.Event('touchmove')
          e.identifier = game._mousedownID
          e._initPosition(x, y)
          that.dispatchEvent(e)
        , false)
        game._element.addEventListener('mouseup', (e) ->
          if not that._mousedown then return
          x = e.pageX
          y = e.pageY
          e = new enchant.Event('touchend')
          e.identifier = game._mousedownID
          e._initPosition(x, y)
          that.dispatchEvent(e)
          that._mousedown = false
        , false)
    ###*
    * DOMのID.
    * @type {String}
    ###
    @define 'id'
      get: -> @_element.id
      set: (id) -> @_element.id = id

    ###*
    * DOMのclass.
    * @type {String}
    ###
    @define 'className'
      get: -> @_element.className
      set: (className) -> @_element.className = className

    ###*
    * Entityの横幅.
    * @type {Number}
    ###
    @define 'width'
      get: -> @_width
      set: (width) -> @_style.width = (@_width = width) + 'px'

    ###*
    * Entityの高さ.
    * @type {Number}
    ###
    @define 'height'
      get: -> @_height
      set: (height) -> @_style.height = (@_height = height) + 'px'

    ###*
    * Entityの背景色.
    * CSSの'color'プロパティと同様の形式で指定できる.
    * @type {String}
    ###
    @define 'backgroundColor'
      get: -> @_backgroundColor
      set: (color) -> @_element.style.backgroundColor = @_backgroundColor = color

    ###*
    * Entityの透明度.
    * 0から1までの値を設定する(0が完全な透明, 1が完全な不透明).
    * @type {Number}
    ###
    @define 'opacity'
      get: -> @_opacity
      set: (opacity) -> @_style.opacity = @_opacity = opacity

    ###*
    * Entityを表示するかどうかを指定する.
    * @type {Boolean}
    ###
    @define 'visible'
      get: -> @_visible
      set: (visible) ->
        if (@_visible = visible)
          @_style.display = 'block'
        else
          @_style.display = 'none'

    ###*
    * Entityのタッチを有効にするかどうかを指定する.
    * @type {Boolean}
    ###
    @define 'touchEnabled'
      get: -> @_touchEnabled
      set: (enabled) ->
        if (@_touchEnabled = enabled)
          @_style.pointerEvents = 'all'
        else
          @_style.pointerEvents = 'none'

    ###*
    * Entityの矩形が交差しているかどうかにより衝突判定を行う.
    * @param {*} other 衝突判定を行うEntityなどx, y, width, heightプロパティを持ったObject.
    * @return {Boolean} 衝突判定の結果.
    ###
    intersect: (other) ->
      @x < other.x + other.width and other.x < @x + @width and
        @y < other.y + other.height and other.y < @y + @height

    ###*
    * Entityの中心点どうしの距離により衝突判定を行う.
    * @param {*} other 衝突判定を行うEntityなどx, y, width, heightプロパティを持ったObject.
    * @param {Number} [distance] 衝突したと見なす最大の距離. デフォルト値は二つのEntityの横幅と高さの平均.
    * @return {Boolean} 衝突判定の結果.
    ###
    within: (other, distance) ->
      if distance is null
        distance = (@width + @height + other.width + other.height) / 4
      (_ = @x - other.x + (@width - other.width) / 2) * _ +
        (_ = @y - other.y + (@height - other.height) / 2) * _ < distance * distance

  ###*
  * @scope enchant.Sprite.prototype
  ###
  class enchant.Sprite extends enchant.Entity
    ###*
    * 画像表示機能を持ったクラス.
    *
    * @example
    *   var bear = new Sprite(32, 32);
    *   bear.image = game.assets['chara1.gif'];
    *
    * @param {Number} [width] Spriteの横幅.
    * @param {Number} [height] Spriteの高さ.
    * @constructs
    * @extends enchant.Entity
    ###
    constructor: (width, height) ->
      super

      @width = width
      @height = height
      @_scaleX = 1
      @_scaleY = 1
      @_rotation = 0
      @_dirty = false
      @_image = null
      @_frame = 0

      @_style.overflow = 'hidden'

      @addEventListener('render', () ->
        if (@_dirty)
          @_style[VENDER_PREFIX + 'Transform'] = [
            'rotate(', @_rotation, 'deg)',
            'scale(', @_scaleX, ',', @_scaleY, ')'
          ].join('')
          @_dirty = false
      )

    ###*
    * Spriteで表示する画像.
    * @type {enchant.Surface}
    ###
    @define 'image'
      get: -> @_image
      set: (image) ->
        if (image is @_image) then return

        if (@_image?)
          if (@_image.css)
            @_style.backgroundImage = ''
          else if (@_element.firstChild)
            @_element.removeChild(@_element.firstChild)
            if (@_dirtyListener)
              @removeEventListener('render', @_dirtyListener)
              @_dirtyListener = null
            else
              @_image._parent = null

        if (image?)
          if (image._css)
            @_style.backgroundImage = image._css
          else if (image._parent)
            canvas = document.createElement('canvas')
            context = canvas.getContext('2d')
            canvas.width = image.width
            canvas.height = image.height
            context.drawImage(image._element, 0, 0)
            @_dirtyListener = ->
              if (image._dirty)
                context.drawImage(image._element)
                image._dirty = false
            @addEventListener('render', @_dirtyListener)
            @_element.appendChild(canvas)
          else
            image._parent = this
            @_element.appendChild(image._element)
        @_image = image

    ###*
    * 表示するフレームのインデックス.
    * Spriteと同じ横幅と高さを持ったフレームがimageプロパティの画像に左上から順に
    * 配列されていると見て, 0から始まるインデックスを指定することでフレームを切り替える.
    * @type {Number}
    ###
    @define 'frame'
      get: -> @_frame
      set: (frame) ->
        @_frame = frame
        row = @_image.width / @_width | 0
        if (@_image._css)
          @_style.backgroundPosition = [
            -(frame % row) * @_width, 'px ',
            -(frame / row | 0) * @_height, 'px'
          ].join('')
        else if (@_element.firstChild)
          style = @_element.firstChild.style
          style.left = -(frame % row) * @_width + 'px'
          style.top = -(frame / row | 0) * @_height + 'px'

    ###*
    * Spriteを拡大縮小する.
    * @param {Number} x 拡大するx軸方向の倍率.
    * @param {Number} [y] 拡大するy軸方向の倍率.
    ###
    scale: (x, y) ->
      if (y is null) then y = x
      @_scaleX *= x
      @_scaleY *= y
      @_dirty = true

    ###*
    * Spriteを回転する.
    * @param {Number} deg 回転する角度 (度数法).
    ###
    rotate: (deg) ->
      @_rotation += deg
      @_dirty = true

    ###*
    * Spriteのx軸方向の倍率.
    * @type {Number}
    ###
    @define 'scaleX'
      get: -> @_scaleX
      set: (scaleX) ->
        @_scaleX = scaleX
        @_dirty = true

    ###*
    * Spriteのy軸方向の倍率.
    * @type {Number}
    ###
    @define 'scaleY'
      get: -> @_scaleY
      set: (scaleY) ->
        @_scaleY = scaleY
        @_dirty = true

    ###*
    * Spriteの回転角 (度数法).
    * @type {Number}
    ###
    @define 'rotation'
      get: -> @_rotation
      set: (rotation) ->
        @_rotation = rotation
        @_dirty = true

  ###*
  * @scope enchant.Label.prototype
  ###
  class enchant.Label extends enchant.Entity
    ###*
    * Labelオブジェクトを作成する.
    * @constructs
    * @extends enchant.Entity
    ###
    constructor: (text) ->
      # FIXME:
      super

      @width = 300
      @text = text

    ###*
    * 表示するテキスト.
    * @type {String}
    ###
    @define 'text'
      get: -> @_element.innerHTML
      set: (text) -> @_element.innerHTML = text

    ###*
    * フォントの指定.
    * CSSの'font'プロパティと同様の形式で指定できる.
    * @type {String}
    ###
    @define 'font'
      get: -> @_style.font
      set: (font) -> @_style.font = font

    ###*
    * 文字色の指定.
    * CSSの'color'プロパティと同様の形式で指定できる.
    * @type {String}
    ###
    @define 'color'
      get: -> @_style.color
      set: (color) -> @_style.color = color

  ###*
  * @scope enchant.Map.prototype
  ###
  class enchant.Map extends enchant.Entity
    ###*
    * タイルセットからマップを生成して表示するクラス.
    *
    * @param {Number} tileWidth タイルの横幅.
    * @param {Number} tileHeight タイルの高さ.
    * @constructs
    * @extends enchant.Entity
    ###
    constructor: (tileWidth, tileHeight) ->
      super

      canvas = document.createElement('canvas')
      if (RETINA_DISPLAY and game.scale is 2)
        canvas.width = game.width * 2
        canvas.height = game.height * 2
        @_style.webkitTransformOrigin = '0 0'
        @_style.webkitTransform = 'scale(0.5)'
      else
        canvas.width = game.width
        canvas.height = game.height
      @_element.appendChild(canvas)
      @_context = canvas.getContext('2d')

      @_tileWidth = tileWidth || 0
      @_tileHeight = tileHeight || 0
      @_image = null
      @_data = [[[]]]
      @_dirty = false
      @_tight = false

      @touchEnabled = false

      ###*
       * タイルが衝突判定を持つかを表す値の二元配列.
       * @type {Array.<Array.<Number>>}
      ###
      @collisionData = null

      @_listeners['render'] = null
      @addEventListener('render', ->
        if (@_dirty or @_previousOffsetX is null)
          @_dirty = false
          @redraw(0, 0, game.width, game.height)
        else if (@_offsetX isnt @_previousOffsetX or @_offsetY isnt @_previousOffsetY)
          if (@_tight)
            x = -@_offsetX
            y = -@_offsetY
            px = -@_previousOffsetX
            py = -@_previousOffsetY
            w1 = x - px + game.width
            w2 = px - x + game.width
            h1 = y - py + game.height
            h2 = py - y + game.height
            if (w1 > @_tileWidth and w2 > @_tileWidth and h1 > @_tileHeight and h2 > @_tileHeight)
              if (w1 < w2)
                sx = 0
                dx = px - x
                sw = w1
              else
                sx = x - px
                dx = 0
                sw = w2

              if (h1 < h2)
                sy = 0
                dy = py - y
                sh = h1
              else
                sy = y - py
                dy = 0
                sh = h2

              if (game._buffer is null)
                game._buffer = document.createElement('canvas')
                game._buffer.width = @_context.canvas.width
                game._buffer.height = @_context.canvas.height

              context = game._buffer.getContext('2d')
              if (@_doubledImage)
                context.clearRect(0, 0, sw*2, sh*2)
                context.drawImage(
                  @_context.canvas, sx*2, sy*2, sw*2, sh*2,
                  0, 0, sw*2, sh*2
                )
                context = @_context
                context.clearRect(dx*2, dy*2, sw*2, sh*2)
                context.drawImage(
                  game._buffer, 0, 0, sw*2, sh*2,
                  dx*2, dy*2, sw*2, sh*2
                )
              else
                context.clearRect(0, 0, sw, sh)
                context.drawImage(
                  @_context.canvas,
                  sx, sy, sw, sh, 0, 0, sw, sh
                )
                context = @_context
                context.clearRect(dx, dy, sw, sh)
                context.drawImage(
                  game._buffer,
                  0, 0, sw, sh, dx, dy, sw, sh
                )

              if (dx is 0)
                @redraw(sw, 0, game.width - sw, game.height)
              else
                @redraw(0, 0, game.width - sw, game.height)

              if (dy is 0)
                @redraw(0, sh, game.width, game.height - sh)
              else
                @redraw(0, 0, game.width, game.height - sh)
            else
              @redraw(0, 0, game.width, game.height)
          else
            @redraw(0, 0, game.width, game.height)
        @_previousOffsetX = @_offsetX
        @_previousOffsetY = @_offsetY
      )
    ###*
    * データを設定する.
    * タイルががimageプロパティの画像に左上から順に配列されていると見て, 0から始まる
    * インデックスの二元配列を設定する.複数指定された場合は後のものから順に表示される.
    * @param {...Array<Array.<Number>>} data タイルのインデックスの二元配列. 複数指定できる.
    ###
    loadData: (data) ->
      @_data = Array.prototype.slice.apply(arguments)
      @_dirty = true

      @_tight = false
      for data in @_data
        c = 0
        for y in data
          for x in data
            if x >= 0 then c++
        if (c / (data.length * data[0].length) > 0.2)
          @_tight = true
          break
    ###*
    * Map上に障害物があるかどうかを判定する.
    * @param {Number} x 判定を行うマップ上の点のx座標.
    * @param {Number} y 判定を行うマップ上の点のy座標.
    * @return {Boolean} 障害物があるかどうか.
    ###
    hitTest: (x, y) ->
      if (x < 0 || @width <= x || y < 0 || @height <= y) then return false

      width = @_image.width
      height = @_image.height
      tileWidth = @_tileWidth || width
      tileHeight = @_tileHeight || height
      x = x / tileWidth | 0
      y = y / tileHeight | 0
      if (@collisionData?)
        return @collisionData[y] and !!@collisionData[y][x]
      else
        for data in @_data
          if (data[y]? and (n = data[y][x])? and
            0 <= n and n < (width / tileWidth | 0) * (height / tileHeight | 0)) then return true
        false
    ###*
    * Mapで表示するタイルセット画像.
    * @type {enchant.Surface}
    ###
    @define 'image'
      get: -> @_image
      set: (image) ->
        @_image = image
        if (RETINA_DISPLAY and game.scale is 2)
          img = new Surface(image.width * 2, image.height * 2)
          tileWidth = @_tileWidth || image.width
          tileHeight = @_tileHeight || image.height
          row = image.width / tileWidth | 0
          col = image.height / tileHeight | 0
          for y in [0...col]
            for x in [0..row]
              img.draw(
                image, x * tileWidth, y * tileHeight, tileWidth, tileHeight,
                x * tileWidth * 2, y * tileHeight * 2, tileWidth * 2, tileHeight * 2
              )
          @_doubledImage = img
        @_dirty = true

    ###*
    * Mapのタイルの横幅.
    * @type {Number}
    ###
    @define 'tileWidth'
      get: -> @_tileWidth
      set: (tileWidth) ->
        @_tileWidth = tileWidth
        @_dirty = true
    ###*
    * Mapのタイルの高さ.
    * @type {Number}
    ###
    @define 'tileHeight'
      get: -> @_tileHeight
      set: (tileHeight) ->
        @_tileHeight = tileHeight
        @_dirty = true

    ###*
    * @private
    ###
    @define 'width'
      get: -> @_tileWidth * @_data[0][0].length

    ###*
    * @private
    ###
    @define 'height'
      get: -> @_tileHeight * @_data[0].length

    ###*
    * @private
    ###
    redraw: (x, y, width, height) ->
      if (@_image is null) then return

      if (@_doubledImage)
        image = @_doubledImage
        tileWidth = @_tileWidth * 2
        tileHeight = @_tileHeight * 2
        dx = -@_offsetX * 2
        dy = -@_offsetY * 2
        x *= 2
        y *= 2
        width *= 2
        height *= 2
      else
        image = @_image
        tileWidth = @_tileWidth
        tileHeight = @_tileHeight
        dx = -@_offsetX
        dy = -@_offsetY
      row = image.width / tileWidth | 0
      col = image.height / tileHeight | 0
      left = Math.max((x + dx) / tileWidth | 0, 0)
      top = Math.max((y + dy) / tileHeight | 0, 0)
      right = Math.ceil((x + dx + width) / tileWidth)
      bottom = Math.ceil((y + dy + height) / tileHeight)

      source = image._element
      context = @_context
      canvas = context.canvas
      context.clearRect(x, y, width, height)
      for data in @_data
        r = Math.min(right, data[0].length)
        b = Math.min(bottom, data.length)
        for y in [top...b]
          for x in [left...r]
            n = data[y][x]
            if (0 <= n and n < row * col)
              sx = (n % row) * tileWidth
              sy = (n / row | 0) * tileHeight
              context.drawImage(
                source, sx, sy, tileWidth, tileHeight,
                x * tileWidth - dx, y * tileHeight - dy, tileWidth, tileHeight
              )

  ###*
  * @scope enchant.Group.prototype
  ###
  class enchant.Group extends enchant.Node
    ###*
    * 複数のNodeを子に持つことができるクラス.
    *
    * @example
    *   var stage = new Group();
    *   stage.addChild(player);
    *   stage.addChild(enemy);
    *   stage.addChild(map);
    *   stage.addEventListener('enterframe', function() {
    *      // playerの座標に従って全体をスクロールする
    *      if (@x > 64 - player.x) {
    *          @x = 64 - player.x;
    *      }
    *   });
    *
    * @constructs
    * @extends enchant.Node
    ###
    constructor:  ->
      super

      ###*
      * 子のNode.
       * @type {Array.<enchant.Node>}
      ###
      @childNodes = []

      @_x = 0
      @_y = 0

    ###*
    * GroupにNodeを追加する.
    * @param {enchant.Node} node 追加するNode.
    ###
    addChild: (node) ->
      @childNodes.push(node)
      node.parentNode = this
      node.dispatchEvent(new enchant.Event('added'))
      if (@scene)
        e = new enchant.Event('addedtoscene')
        node.scene = @scene
        node.dispatchEvent(e)
        node._updateCoordinate()

        fragment = document.createDocumentFragment()
        nodes
        push = Array.prototype.push
        if (node._element)
          fragment.appendChild(node._element)
        else if (node.childNodes)
          nodes = node.childNodes.slice().reverse()
          while (nodes.length)
            node = nodes.pop()
            node.scene = @scene
            node.dispatchEvent(e)
            if (node._element)
              fragment.appendChild(node._element)
            else if (node.childNodes)
              push.apply(nodes, node.childNodes.reverse())

        if not fragment.childNodes.length then return

        thisNode = this
        while (thisNode.parentNode)
          nodes = thisNode.parentNode.childNodes
          nodes = nodes.slice(nodes.indexOf(thisNode) + 1).reverse()
          while (nodes.length)
            node = nodes.pop()
            if (node._element)
              nextSibling = node._element
              break
            else if (node.childNodes)
              push.apply(nodes, node.childNodes.slice().reverse())
          thisNode = thisNode.parentNode
        if (nextSibling)
          @scene._element.insertBefore(fragment, nextSibling)
        else
          @scene._element.appendChild(fragment)

    ###*
    * GroupにNodeを挿入する.
    * @param {enchant.Node} node 挿入するNode.
    * @param {enchant.Node} reference 挿入位置の前にあるNode.
    ###
    insertBefore: (node, reference) ->
      i = @childNodes.indexOf(reference)
      if (i isnt -1)
        @childNodes.splice(i, 0, node)
        node.parentNode = this
        node.dispatchEvent(new enchant.Event('added'))
        if (@scene)
          e = new enchant.Event('addedtoscene')
          node.scene = @scene
          node.dispatchEvent(e)
          node._updateCoordinate()

          fragment = document.createDocumentFragment()
          nodes
          push = Array.prototype.push
          if (node._element)
            fragment.appendChild(node._element)
          else if (node.childNodes)
            nodes = node.childNodes.slice().reverse()
            while (nodes.length)
              node = nodes.pop()
              node.scene = @scene
              node.dispatchEvent(e)
              if (node._element)
                fragment.appendChild(node._element)
              else if (node.childNodes)
                push.apply(nodes, node.childNodes.reverse())
          if not fragment.childNodes.length then return

          thisNode = reference
          while (thisNode.parentNode)
            if (i?)
              nodes = @childNodes.slice(i+1).reverse()
              i = null
            else
              nodes = thisNode.parentNode.childNodes
              nodes = nodes.slice(nodes.indexOf(thisNode) + 1).reverse()
            while (nodes.length)
              node = nodes.pop()
              if (node._element)
                nextSibling = node._element
                break
              else if (node.childNodes)
                push.apply(nodes, node.childNodes.slice().reverse())
            thisNode = thisNode.parentNode
          if (nextSibling)
            @scene._element.insertBefore(fragment, nextSibling)
          else
            @scene._element.appendChild(fragment)
      else
        @addChild(node)
    ###*
    * GroupからNodeを削除する.
    * @param {enchant.Node} node 削除するNode.
    ###
    removeChild: (node) ->
      i = @childNodes.indexOf(node)
      if (i isnt -1)
        @childNodes.splice(i, 1)
      else
        return
      node.parentNode = null
      node.dispatchEvent(new enchant.Event('removed'))
      if (@scene)
        e = new enchant.Event('removedfromscene')
        node.scene = null
        node.dispatchEvent(e)
        if (node._element)
          @scene._element.removeChild(node._element)
        else if (node.childNodes)
          nodes = node.childNodes.slice()
          push = Array.prototype.push
          while (nodes.length)
            node = nodes.pop()
            node.scene = null
            node.dispatchEvent(e)
            if (node._element)
              @scene._element.removeChild(node._element)
            else if (node.childNodes)
              push.apply(nodes, node.childNodes)
    ###*
    * 最初の子Node.
    * @type {enchant.Node}
    ###
    @define 'firstChild'
      get: -> @childNodes[0]
    ###*
    * 最後の子Node.
    * @type {enchant.Node}
    ###
    @define 'lastChild'
      get: -> @childNodes[@childNodes.length-1]

    _updateCoordinate: ->
      if (@parentNode)
        @_offsetX = @parentNode._offsetX + @_x
        @_offsetY = @parentNode._offsetY + @_y
      else
        @_offsetX = @_x
        @_offsetY = @_y
      for child in @childNodes
        child._updateCoordinate()

  ###*
  * @scope enchant.Scene.prototype
  ###
  class enchant.Scene extends enchant.Group
    ###*
    * 表示オブジェクトツリーのルートになるクラス.
    *
    * @example
    *   var scene = new Scene();
    *   scene.addChild(player);
    *   scene.addChild(enemy);
    *   game.pushScene(scene);
    *
    * @constructs
    * @extends enchant.Group
    ###
    constructor: ->
      super

      @_element = document.createElement('div')
      @_element.style.position = 'absolute'
      @_element.style.overflow = 'hidden'
      @_element.style.width = (@width = game.width) + 'px'
      @_element.style.height = (@height = game.height) + 'px'
      @_element.style["#{VENDER_PREFIX}TransformOrigin"] = '0 0'
      @_element.style["#{VENDER_PREFIX}Transform"] = "scale(#{game.scale})"

      @scene = this

      that = this
      if (TOUCH_ENABLED)
        @_element.addEventListener('touchstart', (e) ->
          touches = e.touches
          for touch in touches
            e = new enchant.Event('touchstart')
            e.identifier = touch.identifier
            e._initPosition(touch.pageX, touch.pageY)
            that.dispatchEvent(e)
        , false)
        @_element.addEventListener('touchmove', (e) ->
          touches = e.touches
          for touch in touches
            e = new enchant.Event('touchmove')
            e.identifier = touch.identifier
            e._initPosition(touch.pageX, touch.pageY)
            that.dispatchEvent(e)
        , false)
        @_element.addEventListener('touchend',(e) ->
          touches = e.changedTouches
          for touch in touches
            e = new enchant.Event('touchend')
            e.identifier = touch.identifier
            e._initPosition(touch.pageX, touch.pageY)
            that.dispatchEvent(e)
        , false)
      else
        @_element.addEventListener('mousedown', (e) ->
          x = e.pageX
          y = e.pageY
          e = new enchant.Event('touchstart')
          e.identifier = game._mousedownID
          e._initPosition(x, y)
          that.dispatchEvent(e)
          that._mousedown = true
        , false)
        game._element.addEventListener('mousemove', (e) ->
          if not that._mousedown then return
          x = e.pageX
          y = e.pageY
          e = new enchant.Event('touchmove')
          e.identifier = game._mousedownID
          e._initPosition(x, y)
          that.dispatchEvent(e)
        , false)
        game._element.addEventListener('mouseup', (e) ->
          if not that._mousedown then return
          x = e.pageX
          y = e.pageY
          e = new enchant.Event('touchend')
          e.identifier = game._mousedownID
          e._initPosition(x, y)
          that.dispatchEvent(e)
          that._mousedown = false
        , false)
    ###*
    * Sceneの背景色.
    * CSSの'color'プロパティと同様の形式で指定できる.
    * @type {String}
    ###
    @define 'backgroundColor'
      get: -> @_backgroundColor
      set: (color) -> @_element.style.backgroundColor = @_backgroundColor = color
    _updateCoordinate: ->
      @_offsetX = @_x
      @_offsetY = @_y
      child._updateCoordinate() for child in @childNodes

  CANVAS_DRAWING_METHODS = [
    'putImageData', 'drawImage', 'drawFocusRing', 'fill', 'stroke',
    'clearRect', 'fillRect', 'strokeRect', 'fillText', 'strokeText'
  ]

  ###*
  * @scope enchant.Surface.prototype
  ###
  class enchant.Surface extends enchant.EventTarget
    ###*
    * canvas要素をラップしたクラス.
    *
    * SpriteやMapのimageプロパティに設定して表示させることができる.
    * Canvas APIにアクセスしたいときはcontextプロパティを用いる.
    *
    * @example
    *   // 円を表示するSpriteを作成する
    *   var ball = new Sprite(50, 50);
    *   var surface = new Surface(50, 50);
    *   surface.context.beginPath();
    *   surface.context.arc(25, 25, 25, 0, Math.PI*2, true);
    *   surface.context.fill();
    *   ball.image = surface;
    *
    * @param {Number} width Surfaceの横幅.
    * @param {Number} height Surfaceの高さ.
    * @constructs
    ###
    constructor: (width, height) ->
      super

      ###*
       * Surfaceの横幅.
       * @type {Number}
      ###
      @width = width
      ###*
       * Surfaceの高さ.
       * @type {Number}
      ###
      @height = height
      ###*
       * Surfaceの描画コンテクスト.
       * @type {CanvasRenderingContext2D}
      ###
      @context = null

      id = 'enchant-surface' + game._surfaceID++
      if (document.getCSSCanvasContext)
        @context = document.getCSSCanvasContext('2d', id, width, height)
        @_element = @context.canvas
        @_css = "-webkit-canvas(#{id})"
        context = @context
      else if (document.mozSetImageElement)
        @_element = document.createElement('canvas')
        @_element.width = width
        @_element.height = height
        @_css = '-moz-element(#' + id + ')'
        @context = @_element.getContext('2d')
        document.mozSetImageElement(id, @_element)
      else
        @_element = document.createElement('canvas')
        @_element.width = width
        @_element.height = height
        @_element.style.position = 'absolute'
        @context = @_element.getContext('2d')

        CANVAS_DRAWING_METHODS.forEach((name) ->
          method = @context[name]
          @context[name] = ->
            method.apply(this, arguments)
            @_dirty = true
        , this)
    ###*
    * Surfaceから1ピクセル取得する.
    * @param {Number} x 取得するピクセルのx座標.
    * @param {Number} y 取得するピクセルのy座標.
    * @return {Array.<Number>} ピクセルの情報を[r, g, b, a]の形式で持つ配列.
    ###
    getPixel: (x, y) -> @context.getImageData(x, y, 1, 1).data

    ###*
    * Surfaceに1ピクセル設定する.
    * @param {Number} x 設定するピクセルのx座標.
    * @param {Number} y 設定するピクセルのy座標.
    * @param {Number} r 設定するピクセルのrの値.
    * @param {Number} g 設定するピクセルのgの値.
    * @param {Number} b 設定するピクセルのbの値.
    * @param {Number} a 設定するピクセルの透明度.
    ###
    setPixel: (x, y, r, g, b, a) ->
      pixel = @context.createImageData(1, 1)
      pixel.data[0] = r
      pixel.data[1] = g
      pixel.data[2] = b
      pixel.data[3] = a
      @context.putImageData(pixel, x, y, 1, 1)
    ###*
    * Surfaceの全ピクセルをクリアし透明度0の黒に設定する.
    ###
    clear: ->
      @context.clearRect(0, 0, @width, @height)

    ###*
    * Surfaceに対して引数で指定されたSurfaceを描画する.
    *
    * Canvas APIのdrawImageをラップしており, 描画する矩形を同様の形式で指定できる.
    *
    * @example
    *   var src = game.assets['src.gif'];
    *   var dst = new Surface(100, 100);
    *   dst.draw(src);         // ソースを(0, 0)に描画
    *   dst.draw(src, 50, 50); // ソースを(50, 50)に描画
    *   // ソースを(50, 50)に縦横30ピクセル分だけ描画
    *   dst.draw(src, 50, 50, 30, 30);
    *   // ソースの(10, 10)から縦横40ピクセルの領域を(50, 50)に縦横30ピクセルに縮小して描画
    *   dst.draw(src, 10, 10, 40, 40, 50, 50, 30, 30);
    *
    * @param {enchant.Surface} image 描画に用いるSurface.
    ###
    draw: (image) ->
      arguments[0] = image = image._element
      if (arguments.length is 1)
        @context.drawImage(image, 0, 0)
      else
        @context.drawImage.apply(@context, arguments)

    ###*
    * Surfaceを複製する.
    * @return {enchant.Surface} 複製されたSurface.
    ###
    clone: ->
      clone = new enchant.Surface(@width, @height)
      clone.draw(this)
      clone

    ###*
    * SurfaceからdataスキームのURLを生成する.
    * @return {String} Surfaceを表すdataスキームのURL.
    ###
    toDataURL: ->
      src = @_element.src
      if (src)
        if (src.slice(0, 5) is 'data:')
          return src
        else
          return @clone().toDataURL()
      else
        return @_element.toDataURL()

  ###*
  * 画像ファイルを読み込んでSurfaceオブジェクトを作成する.
  *
  * このメソッドによって作成されたSurfaceはimg要素のラップしておりcontextプロパティに
  * アクセスしたりdraw, clear, getPixel, setPixelメソッドなどの呼び出しでCanvas API
  * を使った画像操作を行うことはできない. ただしdrawメソッドの引数とすることはでき,
  * ほかのSurfaceに描画した上で画像操作を行うことはできる(クロスドメインでロードした
  * 場合はピクセルを取得するなど画像操作の一部が制限される).
  *
  * @param {String} src ロードする画像ファイルのパス.
  * @static
  ###
  enchant.Surface.load = (src) ->
    image = new Image()
    surface = Object.create(Surface.prototype, {
      context: { value: null },
      _css: { value: "url(#{src})" },
      _element: { value: image }
    })
    enchant.EventTarget.call(surface)
    image.src = src
    image.onerror = ->
      throw new Error("Cannot load an asset: #{image.src}")
    image.onload = ->
      surface.width = image.width
      surface.height = image.height
      surface.dispatchEvent(new enchant.Event('load'))
    surface

  ###*
  * @scope enchant.Sound.prototype
  ###
  class enchant.Sound extends enchant.EventTarget
    ###*
    * audio要素をラップしたクラス.
    *
    * MP3ファイルの再生はSafari, Chrome, Firefox, Opera, IEが対応
    * (Firefox, OperaではFlashを経由して再生). WAVEファイルの再生は
    * Safari, Chrome, Firefox, Operaが対応している. ブラウザが音声ファイル
    * のコーデックに対応していない場合は再生されない.
    *
    * コンストラクタではなくenchant.Sound.loadを通じてインスタンスを作成する.
    *
    * @constructs
    ###
    constructor: ->
      super
      throw new Error("Illegal Constructor")

      ###*
       * Soundの再生時間 (秒).
       * @type {Number}
      ###
      @duration = 0
    ###*
    * 再生を開始する.
    ###
    play: -> if @_element? then @_element.play()
    ###*
    * 再生を中断する.
    ###
    pause: -> if @_element? then @_element.pause()
    ###*
    * 再生を停止する.
    ###
    stop: ->
      @pause()
      @currentTime = 0
    ###*
    * Soundを複製する.
    * @return {enchant.Sound} 複製されたSound.
    ###
    clone: ->
      if (@_element instanceof Audio)
        clone = Object.create(enchant.Sound.prototype, {
          _element: { value: @_element.cloneNode(false) },
          duration: { value: @duration }
        })
      else
        clone = Object.create(enchant.Sound.prototype)
      enchant.EventTarget.call(clone)
      clone
    ###*
    * 現在の再生位置 (秒).
    * @type {Number}
    ###
    @define 'currentTime'
      get: -> if @_element? then @_element.currentTime else 0
      set: (time) -> if @_element? then @_element.currentTime = time

    ###*
    * ボリューム. 0 (無音) ～ 1 (フルボリューム).
    * @type {Number}
    ###
    @define 'volume'
      get: -> if @_element? then @_element.volume else 1
      set: (volume) -> if @_element? then @_element.volume = volume

  ###*
  * 音声ファイルを読み込んでSurfaceオブジェクトを作成する.
  *
  * @param {String} src ロードする音声ファイルのパス.
  * @param {String} [type] 音声ファイルのMIME Type.
  * @static
  ###
  enchant.Sound.load = (src, type) ->
    if (type is null)
      ext = src.match(/\.\w+$/)[0]
      if (ext)
        type = "audio/#{ext.slice(1).toLowerCase()}"
      else
        type = ''
    type = type.replace('mp3', 'mpeg')

    sound = Object.create(enchant.Sound.prototype)
    enchant.EventTarget.call(sound)
    audio = new Audio()
    if (!enchant.Sound.enabledInMobileSafari and VENDER_PREFIX is 'webkit' and TOUCH_ENABLED)
      window.setTimeout(->
        sound.dispatchEvent(new enchant.Event('load'))
      , 0)
    else
      if (audio.canPlayType(type))
        audio.src = src
        audio.load()
        audio.autoplay = false
        audio.onerror = -> throw new Error("Cannot load an asset: #{audio.src}")
        audio.addEventListener('canplaythrough', ->
          sound.duration = audio.duration
          sound.dispatchEvent(new enchant.Event('load'))
        , false)
        sound._element = audio
      else if (type is 'audio/mpeg')
        embed = document.createElement('embed')
        id = 'enchant-audio' + game._soundID++
        embed.width = embed.height = 1
        embed.name = id
        embed.src = "sound.swf?id=#{id}&src=#{src}"
        embed.allowscriptaccess = 'always'
        embed.style.position = 'absolute'
        embed.style.left = '-1px'
        sound.addEventListener('load', ->
          Object.defineProperties(embed, {
            currentTime: {
              get: -> embed.getCurrentTime(),
              set: (time) -> embed.setCurrentTime(time)
            },
            volume: {
              get: -> embed.getVolume(),
              set: (volume) -> embed.setVolume(volume)
            }
          })
          sound._element = embed
          sound.duration = embed.getDuration()
        )
        game._element.appendChild(embed)
        enchant.Sound[id] = sound
      else
        window.setTimeout(->
          sound.dispatchEvent(new enchant.Event('load'))
        , 0)
    sound

  enchant.Sound.enabledInMobileSafari = false
)()

window.enchant = enchant
