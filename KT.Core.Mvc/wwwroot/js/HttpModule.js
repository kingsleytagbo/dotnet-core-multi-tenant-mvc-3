const HttpModule = (function () {
    /* Http Module used for making Http api calls */
    const API_URL = 'https://localhost:44373';
    const public_key = "d62c03a2-57b6-4e14-8153-d05d3aa9ab10";

    return {

        register: function (model) {
            model.Accept = 'application/json';
            model['Content-Type'] = 'application/json; charset=utf-8';
            model['auth_site'] = public_key;

            const headers = model;
            const body = JSON.stringify({});
            return this.post('/api/account/register', headers, body);
        },

        login: function (username, password) {
            const headers = {
                Accept: 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                auth_site: public_key, "UserName": username, Password: password, RememberMe: true
            };
            const body = JSON.stringify({});
            return this.post('/api/account/login',headers, body);
        },

        getUsers: function () {
            const headers = {
                Accept: 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                auth_site: public_key
            };
            const body = {};
            return this.post('/api/account/getusers', headers, body);
        },

        createUser: function (user) {
            const email = user.user_login;
            const password = user.user_pass;
            const newUser = {
                user_login: email, user_pass: password, user_nicename: password, user_email: email, display_name: email,
                user_status: 1, user_registered: 1, user_url: '', user_activation_key: '', spam: 0,
                deleted: 0, site_id: 1
            };
            const body = {
                "user": newUser
            };
            // console.log({ 'createUser': user, newUser: newUser});
            return this.post('/users/createUser', body);
        },

        updateUser: function (user) {
            // console.log({ 'updateUser': user });
            const body = {
                "user": {
                    user
                }
            };
            return this.post('/users/updateUser', body);
        },

        getImages: function (token) {
            const headers = {
                Accept: 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                auth_site: public_key,
                'Authorization': 'Bearer ' + token
            };
            const body = {};
            console.log(headers);
            return this.get('/api/images', headers, body);
        },

        createImage: function (token, model) {
            model.Accept = 'application/json';
            model['Content-Type'] = 'application/json; charset=utf-8';
            model['auth_site'] = public_key;
            model['Authorization'] = 'Bearer ' + token;

            const headers = model;
            const body = JSON.stringify({});
            return this.post('/api/images', headers, body);
        },

        post: function (destination, headers, body) {
            const url = `${API_URL}${destination}`;
            //console.log({ url: url, headers: headers, body: JSON.stringify(body) });
      
            const result = fetch(url, {
                method: 'POST',
                headers: headers,
                body: JSON.stringify(body),
            });
            const response = result;
            return response;
        },

        get: function (destination, headers, body) {
            const url = `${API_URL}${destination}`;
            // console.log({ url: url, headers: headers, body: JSON.stringify(body) });

            const result = fetch(url, {
                method: 'GET',
                headers: headers
            });
            const response = result;
            return response;
        }

    }

}());