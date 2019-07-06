function neshanmap(options) {
    var defaults = {
        tagName: '',
        latitude: null,
        longitude: null,
        siteName: ''
    };

    options = $.extend(defaults, options);

    var myMap = new L.Map(options.tagName,
        {
            key: 'web.oRd14Kh8WgB1wz3J8pluhy9cEHcqWTRlK5nPQEYM',
            maptype: 'dreamy',
            poi: true,
            traffic: false,
            center: [options.latitude, options.longitude],
            zoom: 14
        });

    var marker = new L.marker([options.latitude, options.longitude]).addTo(myMap);

    marker.bindPopup("<b>" + options.siteName + "</b>").openPopup();

    return this;
};