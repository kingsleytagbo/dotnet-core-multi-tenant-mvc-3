const SiteModule = (function (Page) {
    return {
        init: function () {
            Page.toggleAdminMenu();
        }
    };
}(PageModule));

 $(document).ready(function () {
     console.log(SiteModule.init());
     //SiteModule.loadCategories();
 });
