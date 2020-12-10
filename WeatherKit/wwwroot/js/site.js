
let dropdown = $('#city');
var textValue = '';

const url = '/Home/CityList';

// Populate dropdown with list of cities
$.getJSON(url, function (data) {
    $.each(data, function (key, entry) {
        if (entry.state === '') {
            textValue = [entry.name, entry.country].join(', ');
        }
        else {
            textValue = [entry.name, entry.state, entry.country].join(', ');
        }

        dropdown.append($('<option></option>').attr('value', textValue).text(textValue));
    })
});
