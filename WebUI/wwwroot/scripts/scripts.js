"use strict";

$(() => {
  const input = document.querySelector("#phone");
  if (input) {
    window.intlTelInput(input, {
      autoHideDialCode: false,
      initialCountry: "sa",
      preferredCountries: [],
      separateDialCode: true,
    });
  }

  const selectField = $(".select__field");
  if (selectField.length > 0) {
    selectField.select2({
      theme: "bootstrap-5",
    });
  }
});
