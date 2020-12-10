// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//import { data } from "jquery";

// Write your JavaScript code.

let dropdown = $('#city');
var textValue = '';

const url = '/Home/CityList';

// Populate dropdown with list of cities
//$("#cityInput").on("input", function () {
    //console.log("Event caught");
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
//});
