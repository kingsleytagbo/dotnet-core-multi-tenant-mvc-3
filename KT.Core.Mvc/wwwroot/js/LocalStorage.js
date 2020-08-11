﻿/**
 *  Manages LocalStorage data operations
 *  Author: Kingsley Tagbo
*/
const database = new function () {

    /* saves one item or a list of items */
    this.save = function (key, data) {
        let items = database.merge(key, data);
        //console.log({key: key, model: items});
        localStorage.setItem(key, JSON.stringify(items));
    };

    /* retrieves one item that matches a key */
    this.fetch = function (key) {
        let data = localStorage.getItem(key);
        if (data) {
            return database.sort(JSON.parse(data));
        }
        else {
            database.removeAll(key);
            return [];
        }
    };

    this.sort = function (data) {
        let sortSorted = data.sort(function (first, second) {
            if (first.sort === second.sort) {
                return (second.lastName > first.lastName ? 1 : -1);
            }
            return (second.sort - first.sort);
        });
        return sortSorted;
    }

    /* syncs both additions or modifications of data */
    this.merge = function (key, data) {
        let items = localStorage.getItem(key);
        let dataItems = [];
        if (items && data) {
            dataItems = JSON.parse(items);
            let item = data;
            let found = false;

            if ((!item.Id) || (item.Id.trim().length === 0) || (item.Id.trim() === '')) {
                item.Id = database.getGuid();
            }

            if (dataItems.length > 0) {
                for (let d = 0; d < dataItems.length; d++) {
                    if (dataItems[d].Id === item.Id) {
                        found = true;
                        dataItems[d] = item;
                        break;
                    }
                };
            }
            if (!found) {
                dataItems.push(item);
            }
        }
        return dataItems;
    }

    /* selects one item that matches the item's key */
    this.selectOne = function (itemKey, itemsKey) {
        let items = localStorage.getItem(itemsKey);
        let match = null;
        if (items) {
            let dataItems = JSON.parse(items);
            let i = dataItems.length
            while (i--) {
                if (String(itemKey) === String(dataItems[i][itemsKey])) {
                    match = dataItems[i];
                    break;
                }
            }
        }
        return match;
    }

    /* removes one item that matches the key */
    this.removeOne = function (key, data) {
        let items = localStorage.getItem(key);
        let match = false;
        if (items && data) {
            let dataItems = JSON.parse(items);
            let i = dataItems.length
            while (i--) {
                if (data.Id === dataItems[i].Id) {

                    dataItems.splice(i, 1);

                    localStorage.setItem(key, JSON.stringify(dataItems));
                    match = true;
                    break;
                }
            }
        }
        return match;
    }

    /* removes all items for a key and resets to an empty array */
    this.removeAll = function (key) {
        localStorage.setItem(key, JSON.stringify([]));
    }

    /* generates a unique identifier similar to a Guid */
    this.getGuid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

};