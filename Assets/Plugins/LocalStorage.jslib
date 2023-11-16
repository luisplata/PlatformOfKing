mergeInto(LibraryManager.library, {

  SetLocalStorageValue: function (key, value) {
     localStorage.setItem(key, value);
  },

  GetLocalStorageValue: function (key) {
       return localStorage.getItem(key);
   },
});