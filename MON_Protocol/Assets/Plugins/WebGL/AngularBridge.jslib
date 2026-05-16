mergeInto(LibraryManager.library, {
  SendMessageToAngular: function(jsonPtr) {
    var json = UTF8ToString(jsonPtr);
    var data = JSON.parse(json);
    window.postMessage(data, "*");
  }
});
