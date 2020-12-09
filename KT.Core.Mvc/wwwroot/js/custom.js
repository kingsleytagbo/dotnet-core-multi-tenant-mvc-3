const SiteModule = (function (Page) {
    return {
        init: function () {
            Page.toggleAdminMenu();
        }
    };
}(PageModule));

 $(document).ready(function () {
     SiteModule.init();
     //SiteModule.loadCategories();
 });
