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

//Form gets submitted
$('.fsForm').submit(function (e) {
	if ($("#" + e.target.id).valid() === true) {
		e.preventDefault();

		$("#loadMe").modal("show");
		var formData = new FormData(this);

		var f = $('input[type=file]')[0].files[0];
		formData.append("file", f);
		var selectedOutputFormat = $("#selOutputFormat").val();
		formData.append("selectedOutputFormat", selectedOutputFormat);

		$.ajax({
			type: 'post',
			url: this.action,
			data: formData,
			processData: false,
			contentType: false
		}).done(function (result) {
			setTimeout(function () {
					$("#loadMe").modal("hide");
				},
				1000);
			$("#divContent").html(result);
		});

	} else {
		e.preventDefault();
	}
});

//Selection has changed
function fSelectChanged(selObject) {
	hideAll();
	if (selObject.value > 0) {
		$("#" + selObject.value).show();
		$("#" + selObject.value).validate();
		addRules(selObject.value);
		hideShowAllFilters();
	}
}

//Selection of the expert level has changed
function selExpertSelectChange() {
	hideShowAllFilters();
}

//Hide an show filters from Experience level
function hideShowAllFilters() {
	var selectedExpertLevel = $("#selExpertLevel").val();

	var myClasses = document.querySelectorAll(".filteritem");
	i = 0,
		l = myClasses.length;

	for (i; i < l; i++) {
		if (selectedExpertLevel === "2") {
			myClasses[i].style.display = 'none';
		} else {
			myClasses[i].style.display = 'block';
		}
	}
}

//Hide items and destroy the validator
function hideAll() {
	var myClasses = document.querySelectorAll('.fsForm'),
		i = 0,
		l = myClasses.length;

	for (i; i < l; i++) {
		myClasses[i].style.display = 'none';
		$("#" + myClasses[i].id).validate().destroy();
	}
}