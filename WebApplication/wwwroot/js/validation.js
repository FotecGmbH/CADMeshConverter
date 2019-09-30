// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        13.09.2019 10:15
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

//Add rules for the validation
function addRules(id) {
    if ($("#" + id + " .float").length > 0) {
        $("#" + id + " .float").rules("add", {
		    required: false,
		    number: true,
		    messages: {
			    number: "Only digits allowed!"
		    }
	    });
    }

    if ($("#" + id + " .integer").length > 0) {
	    $("#" + id + " .integer").rules("add", {
		    required: false,
		    digits: true,
		    messages: {
			    digits: "Must be a number!"
		    }
	    });
    }
}