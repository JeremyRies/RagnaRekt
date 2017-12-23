/**
 * A view manager for both screen and controller
 * @params {AirConsole} airconsole - The airconsole instance
 * @constructor
 */
var AirConsoleViewManager = function(airconsole) {
  this.airconsole = airconsole;
  this.views = {};
  this.current_view = {
    self: null,
    ctrl: null,
    screen: null
  };
  this.class_start = 'default-view';
  this.is_screen = airconsole.getDeviceId() === AirConsole.SCREEN;
  this.setupViews_();
};

AirConsoleViewManager.prototype = {

  /**
   * Called to setup the <div class="view"></div>
   * @private
   */
  setupViews_: function() {
    var start_view_id = null;
    var views = document.querySelectorAll('.view');
    for (var i = 0; i < views.length; i++) {
      var view = views[i];
      var id = view.id;
      this.views[id] = view;
      var is_start = view.className.indexOf(this.class_start) > -1;
      if (is_start) {
        start_view_id = id;
      }
    }
    this.hideAll();
    if (start_view_id) {
      this.show(start_view_id);
    }
  },

  /**
   * Called onDeviceStateChange
   * @param {Object} custom_data - The device user data
   * @param {Function} cb - A callback function. Gets called when view was changed
   */
  onViewChange: function(custom_data, cb) {
    if (!custom_data) return;
    var screen_view = custom_data.screen_view;
    var ctrl_view = custom_data.ctrl_view;
    var view_id = this.is_screen ? screen_view : ctrl_view;
    if (view_id) {
      if (this.show(view_id) && typeof cb === 'function') {
        cb(view_id);
      }
    }
  },

  /**
   * Sets the device custom state
   * @param {String} key
   * @param {Mixed} value
   */
  setState: function(key, value) {
    this.airconsole.setCustomDeviceStateProperty(key, value);
  },

  /**
   * Triggers a view change on all controllers
   * @param {String} view - The view id
   * @param {Boolean} except_me - This controller will not change the view
   */
  controllersShow: function(view, except_me) {
    if (!view) return;
    this.setState('ctrl_view', view);
    if (!this.is_screen && !except_me) {
      this.show(view);
    }
  },

  /**
   * Triggers a view change on the screen
   * @param {String} view - The view id
   */
  screenShow: function(view) {
    if (!view) return;
    this.setState('screen_view', view);
    if (this.is_screen) {
      this.show(view);
    }
  },

  /**
   * Triggers a view change on both the screen and controllers
   * @param {String} view - The view id
   */
  allShow: function(view) {
    this.screenShow(view);
    this.controllersShow(view);
  },

  /**
   * Shows a view and hides all others
   * @param {String} view - The view id
   */
  show: function(id) {
    var state = false;
    if (this.current_view.self !== id) {
      var view = this.views[id];
      if (view) {
        this.current_view.self = id;
        this.hideAll();
        view.style.display = "block";
        state = true;
      } else {
        console.warn("Could not find view with ID:", id);
      }
    }
    return state;
  },

  /**
   * Hides all views
   */
  hideAll: function() {
    for (var key in this.views) {
      this.views[key].style.display = "none";
    }
  }

};