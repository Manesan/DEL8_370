//search bar
function searchCards() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilter");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCards");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRAapproved() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRAapproved");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsApproved");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRArejected() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRArejected");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsRejected");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRAextension() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRAextension");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsExtension");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRArenewal() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRArenewal");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsRenewal");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}


function searchRArejected() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRArejected");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsRejected");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRAapprovede() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRAapprovede");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsApprovedE");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRAapprovedr() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRAapprovedr");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsApprovedR");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}


$(document).ready(function () {
  if (!$.browser.webkit) {
      $('.wrapper').html('<p>Sorry! Non webkit users. :(</p>');
  }
});


//Browse properties filters:
function filterDefault(){
  document.getElementById("filterCategory").innerHTML = 'Default';
}
function filterLTH(){
  document.getElementById("filterCategory").innerHTML = 'Low to High';
}
function filterHTL(){
  document.getElementById("filterCategory").innerHTML = 'High to Low';
}
function filterRecent(){
  document.getElementById("filterCategory").innerHTML = 'Most Recent';
}
function filterType(){
  document.getElementById("filterCategory").innerHTML = 'Property Type';
}


//back button
function goBack() {
  window.history.back();
}


function hideDiv(){
  var x = document.getElementById("moreOptions");
  if (x.style.display === "none") {
    x.style.display = "block";
  } else {
    x.style.display = "none";

  }
}


function editPointOfInterest(){
  //edit header text
  document.getElementById("title").innerHTML = "Update Point of Interest";

  //hide edit button
  document.getElementById("edit").style.display = "none";
  document.getElementById("pointTypeDescriptionInput").style.display = "none";
  document.getElementById("suburbNameInput").style.display = "none";

  //make OK button visible
  document.getElementById("OKButton").display ==="block" ;
  OKButton.style.visibility = "visible";
  document.getElementById("typeInput").style.visibility = "visible";
  document.getElementById("suburbInput").style.visibility = "visible";

  document.getElementById("prompt").style.display = "block";

  //enable input fields
  document.getElementById("description").disabled = false;
  document.getElementById("typeInput").disabled = false;
  document.getElementById("suburbInput").disabled = false;
}


function editValuation() {
  //edit header text
  document.getElementById("title").innerHTML = "Capture Valuation";

  //hide edit button
  document.getElementById("edit").style.display = "none";

  //make upload doc button visible
  document.getElementById("uploadButton").style.display ="block" ;
  uploadButton.style.visibility = "visible";

  //make OK button visible
  document.getElementById("OKButton").style.visibility = "visible" ;

  //make labels visible
  document.getElementById("label1").style.display ="block" ;
  label1.style.visibility = "visible";

  //enable input fields
  document.getElementById("input2").disabled = false;
  document.getElementById("input3").disabled = false;


  document.getElementById("prompt").innerHTML = "Please provide the valuation details";
  document.getElementById("downloadDocument").style.display = "none" ;
  //document.getElementById("valuerInput").style.visibility = "visible";
  //document.getElementById("input4").style.display = "none";
  //document.getElementById("valuationStatusInput").style.visibility = "visible";
  //document.getElementById("input6").style.display = "none";
}


function editRentalAgreement() {
  //edit header text
  document.getElementById("title").innerHTML = "Maintain Rental Agreement";

  //hide edit button
  document.getElementById("edit").style.display = "none";

  //make OK button visible
  document.getElementById("OKButton").display ==="block" ;
  OKButton.style.visibility = "visible";

  //make labels visible
  document.getElementById("label1").display ==="block" ;
  label1.style.visibility = "visible";
  document.getElementById("label2").display ==="block" ;
  label2.style.visibility = "visible";
  document.getElementById("label3").display ==="block" ;
  label3.style.visibility = "visible";

  //make upload button visible
  document.getElementById("uploadButton1").display ==="block" ;
  uploadButton1.style.visibility = "visible";
}


//registration radio button options for id or passport
function southAfrican(){
  document.getElementById("idnumber").placeholder = "ID Number";
  document.getElementById("idnumber").id = "idnumber";
}
function notSouthAfrican(){
  document.getElementById("idnumber").placeholder = "Passport Number";
  document.getElementById("idnumber").id = "passport";
}


//convert address into google maps link
function addressLink(){
  $("address").each(function(){
    var embed ="<iframe width=100% height='100' frameborder='0' scrolling='no'  marginheight='0' marginwidth='0'   src='https://maps.google.com/maps?&amp;q="+ encodeURIComponent( $(this).text() ) +"&amp;output=embed'></iframe>";
                                $(this).html(embed);
   });
};


//-----------------------------------------------------------//DANIEL JS:
function editInspection(statusid) {
  //edit header texts
  if (statusid == 1){
    document.getElementById("title").innerHTML = "Capture Inspection";
    document.getElementById("prompt").innerHTML = "Please provide the inspection details";
  }
  else if (statusid == 2){
    document.getElementById("title").innerHTML = "Follow Up On Inspection";
    document.getElementById("prompt").innerHTML = "Please provide the updated inspection details";
  }

  //hide controls
  document.getElementById("edit").style.display = "none";
  document.getElementById("downloadDocument").style.display = "none" ;
  //document.getElementById("inspectorNameSurnameInput").style.display = "none"; --hides the inspector dropdown
  document.getElementById("inspectionTypeDescriptionInput").style.display = "none";
  document.getElementById("inspectionStatusDescriptionInput").style.display = "none";
  if (document.getElementsByName("defectDescription") != null){
    document.getElementsByName("defectDescription").forEach(e => {
      e.style.display = "none";
    });
  }
  if (document.getElementsByName("defectSpace") != null){
    document.getElementsByName("defectSpace").forEach(e => {
      e.style.display = "none";
    });
  }

  //show controls
  document.getElementById("uploadDocumentLabel").style.display = "block";
  document.getElementById("uploadButton").style.display = "block";
  document.getElementById("OKButton").style.visibility = "visible";
  // document.getElementById("inspectorInput").style.visibility = "visible";
  document.getElementById("statusInput").style.visibility = "visible";
  document.getElementById("inspectionTypeInput").style.visibility = "visible";
  document.getElementById("inspectionTypeInput").style.display = "block";
  document.getElementById("statusInput").style.display = "block";
  if (document.getElementsByName("defectDescriptionInput") != null){
    document.getElementsByName("defectDescriptionInput").forEach(e => {
      e.style.display = "initial";
    });
  }
  if (document.getElementsByName("defectSpaceInput") != null){
    document.getElementsByName("defectSpaceInput").forEach(e => {
      e.style.display = "initial";
      console.log("hit", e.style.display)
    });
  }

  //enable input fields
  document.getElementById("defectWrapper1").hidden = false;
 document.getElementById("noPointer").style.pointerEvents = "auto";

  //document.getElementById("defectSpaceInput").disabled = false;
  document.getElementById("commentsInput").disabled = false;
  ////document.getElementById("inspectorInput").disabled = false;
  document.getElementById("statusInput").disabled = false;

  //document.getElementById("defectDescriptionInput").disabled = false;
  document.getElementById("inspectionTypeInput").disabled = false;
  //document.getElementById("propertyDefectTotalQuantityInput").disabled = false;
  document.getElementById("date").disabled = false;
}



function editProperty() {
  //edit header text
  document.getElementById("title").innerHTML = "Update Property";

  //hide edit button
  document.getElementById("editProperty").style.display = "none";
  document.getElementById("mandateTypeDescriptionInput").style.display = "none";
  document.getElementById("marketTypeDescriptionInput").style.display = "none";
  document.getElementById("propertyTypeDescriptionInput").style.display = "none";
  document.getElementById("suburbNameInput").style.display = "none";

  //make OK button visible
  document.getElementById("OKButton").style.visibility = "visible";
  document.getElementById("prompt").style.display = "block";
  document.getElementById("mandateTypeInput").style.visibility = "visible";
  document.getElementById("marketTypeInput").style.visibility = "visible";
  document.getElementById("propertyTypeInput").style.visibility = "visible";
  document.getElementById("suburbInput").style.visibility = "visible";

  //enable input fields
  document.getElementById("mandateDateInput").disabled = false;
  document.getElementById("mandateTypeInput").disabled = false;

  document.getElementById("priceInput").disabled = false;
  document.getElementById("marketTypeInput").disabled = false;
  document.getElementById("propertyTypeInput").disabled = false;
  document.getElementById("termInput").disabled = false;
  document.getElementById("startDateInput").disabled = false;

  document.getElementById("addressInput").disabled = false;
  document.getElementById("suburbInput").disabled = false;

  document.getElementById("OwnerNameInput").disabled = false;
  document.getElementById("OwnerSurnameInput").disabled = false;
  document.getElementById("OwnerEmailInput").disabled = false;
  document.getElementById("OwnerIDInput").disabled = false;
  document.getElementById("OwnerAddressInput").disabled = false;
  document.getElementById("OwnerContactNumberInput").disabled = false;
  document.getElementById("OwnerAltContactNumberInput").disabled = false;

  document.getElementById("bedroomsInput").disabled = false;
  document.getElementById("bathroomsInput").disabled = false;

  document.getElementById("featureAcceptInput").disabled = false;
  document.getElementById("otherBuildingAcceptInput").disabled = false;
  document.getElementById("pointAcceptInput").disabled = false;
}


// Grouped functionality for single-attribute CRUDs
function editSingleAttributeCRUD(objectName) {

  // edit header text
  document.getElementById("title").innerHTML = "Update " + objectName;

  //hide control
  document.getElementById("edit").style.display = "none";

  //show control
  document.getElementById("OKButton").style.visibility = "visible";
  document.getElementById("prompt").style.display = "block";

  //enable input field
  document.getElementById("descriptionInput").disabled = false;
}

function editDoubleAttributeCRUD(objectName) {

  // edit header text
  document.getElementById("title").innerHTML = "Update " + objectName;

  //hide control
  document.getElementById("edit").style.display = "none";

  //show control
  document.getElementById("OKButton").style.visibility = "visible";
  document.getElementById("prompt").style.display = "block";

  //enable input field
  document.getElementById("descriptionInput").disabled = false;
  document.getElementById("dropdownDisplay").style.display = "none";
  document.getElementById("dropdownInput").style.display = "block";
}


//----------------------------------------//LANI JS:
// Employee
function editEmployee() {
  //edit header text
  document.getElementById("title").innerHTML = "Update Employee";

  //hide control
  document.getElementById("edit").style.display = "none";

  //document.getElementById("checkBoxInput").style.display = "block"; //hides the label
  //document.getElementById("employeeTypeInput").disabled = false; //enable the checkboxes
  //document.getElementById("employeeTypeInput").style.display = "block"; //show the checkboxes
  //document.getElementById("employeeTypeDescriptionInput").style.display = "initial";

  //show control
  document.getElementById("OKButton").style.visibility = "visible";
  document.getElementById("prompt").style.display = "block";

  //enable input fields
  document.getElementById("name").disabled = false;
  document.getElementById("surname").disabled = false;
  document.getElementById("contactnumber").disabled = false;
  document.getElementById("altcontactnumber").disabled = false;
  document.getElementById("email").disabled = false;
  //document.getElementById("idorpassportnumber").disabled = false;
  document.getElementById("address").disabled = false;
  document.getElementById("banking").disabled = false;
  document.getElementById("dateemployed").disabled = false;
  document.getElementById("renumeration").disabled = false;
  document.getElementById("EmployeeTypeWrapper1").hidden = false;
  document.getElementById("noPointer").style.pointerEvents = "auto";
  console.log(document.getElementById("noPointer").style.pointerEvents)
}

//...........................................//Rotondwa's JS:
function editPropertyOwner() {
  //edit header text
  document.getElementById("title").innerHTML = "Update Property Owner";

  //hide edit button
  document.getElementById("editPropertyOwner").style.display = "none";

  //make OK button visible
  document.getElementById("OKButton").style.visibility = "visible";
  document.getElementById("prompt").style.display = "block";

  //enable input fields
  document.getElementById("nameInput").disabled = false;
  document.getElementById("surnameInput").disabled = false;
  document.getElementById("emailInput").disabled = false;
  //document.getElementById("numberInput").disabled = false;
  document.getElementById("addressInput").disabled = false;
  document.getElementById("contactnumberInput").disabled = false;
  document.getElementById("altcontactnumberInput").disabled = false;
}


function editProperty() {
  //edit header text
  document.getElementById("title").innerHTML = "Update Property";

  //hide edit button
  document.getElementById("editProperty").style.display = "none";
  document.getElementById("mandateTypeDescriptionInput").style.display = "none";
  document.getElementById("marketTypeDescriptionInput").style.display = "none";
  document.getElementById("propertyTypeDescriptionInput").style.display = "none";
  document.getElementById("suburbNameInput").style.display = "none";

  //make OK button visible
  document.getElementById("OKButton").style.visibility = "visible";
  document.getElementById("prompt").style.display = "block";
  document.getElementById("mandateTypeInput").style.visibility = "visible";
  document.getElementById("marketTypeInput").style.visibility = "visible";
  document.getElementById("propertyTypeInput").style.visibility = "visible";
  document.getElementById("suburbInput").style.visibility = "visible";

  //enable input fields
  document.getElementById("mandateDateInput").disabled = false;
  document.getElementById("mandateTypeInput").disabled = false;

  document.getElementById("priceInput").disabled = false;
  document.getElementById("marketTypeInput").disabled = false;
  document.getElementById("propertyTypeInput").disabled = false;
  document.getElementById("termInput").disabled = false;
  document.getElementById("startDateInput").disabled = false;

  document.getElementById("addressInput").disabled = false;
  document.getElementById("suburbInput").disabled = false;

  document.getElementById("OwnerNameInput").disabled = false;
  document.getElementById("OwnerSurnameInput").disabled = false;
  document.getElementById("OwnerEmailInput").disabled = false;
  document.getElementById("OwnerIDInput").disabled = false;
  document.getElementById("OwnerAddressInput").disabled = false;
  document.getElementById("OwnerContactNumberInput").disabled = false;
  document.getElementById("OwnerAltContactNumberInput").disabled = false;

  document.getElementById("bedroomsInput").disabled = false;
  document.getElementById("bathroomsInput").disabled = false;

  document.getElementById("featureAcceptInput").disabled = false;
  document.getElementById("otherBuildingAcceptInput").disabled = false;
  document.getElementById("pointAcceptInput").disabled = false;
}


function editPointOfInterest(){
   //edit header text
   document.getElementById("title").innerHTML = "Update Point of Interest";

   //hide edit button

   document.getElementById("suburbNameInput").style.display = "none";

   //make OK button visible
   document.getElementById("OKButton").display ==="block" ;
   OKButton.style.visibility = "visible";

   document.getElementById("prompt").style.display = "block";

   document.getElementById("edit").style.display = "none";
   document.getElementById("typeInput").style.display = "block";
   document.getElementById("typeInput").disabled = false;

   document.getElementById("suburbNameInput").style.display = "none";
   document.getElementById("suburbInput").disabled = false;
   document.getElementById("suburbInput").style.display = "block";

   document.getElementById("pointTypeNameInput").style.display = "none";
   document.getElementById("nameInput").style.visibility = "visible";
   document.getElementById("nameInput").disabled = false;
}



function submitAddJS(){
  $("#addModal").modal('hide');
  $("#confirmAddModal").modal('show');
}




//Validation check
  // Example starter JavaScript for disabling form submissions if there are invalid fields
  (function() {
    'use strict';
    window.addEventListener("load", function() {
      // Fetch all the forms we want to apply custom Bootstrap validation styles to
      var forms = document.getElementsByClassName('needs-validation');
      // Loop over them and prevent submission
      var validation = Array.prototype.filter.call(forms, function(form) {
        form.addEventListener('submit', function(event) {
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            console.log("invalid")
          }
          form.classList.add('was-validated');
        }, false);
      });
    }, false);
  })();



//Rental agreements search bars
function searchRented() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRented");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsRented");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchPendingAgentTermination() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterPendingAgentTermination");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsPendingAgentTermination");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchPendingClientAcceptance() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterPendingClientAcceptance");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsPendingClientAcceptance");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRentalRenewed() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRentalRenewed");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsRentalRenewed");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchRentalExtended() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterRentalExtended");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsRentalExtended");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchPendingClientExtensionAcceptance() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterPendingClientExtensionAcceptance");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsPendingClientExtensionAcceptance");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchPendingClientRenewalAcceptance() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterPendingClientRenewalAcceptance");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsPendingClientRenewalAcceptance");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}

function searchPendingAgentExtension() {
  var input, filter, cards, cardContainer, h5, title, i;
  input = document.getElementById("myFilterPendingAgentExtension");
  filter = input.value.toUpperCase();
  cardContainer = document.getElementById("myCardsPendingAgentExtension");
  cards = cardContainer.getElementsByClassName("card");
  for (i = 0; i < cards.length; i++) {
      title = cards[i].querySelector(".card-body h5.card-title ");
      if (title.innerText.toUpperCase().indexOf(filter) > -1) {
          cards[i].style.display = "";
      } else {
          cards[i].style.display = "none";
      }
  }
}