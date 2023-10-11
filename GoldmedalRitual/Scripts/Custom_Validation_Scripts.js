function validateMobileNumber(inputElement) {
    var regex = /^[0-9]{10}$/;
    var input = inputElement.value;
    if (regex.test(input)) {
        return true;
    } else {
        alert("Please enter a valid 10 digit mobile number");
        inputElement.value = '';
        return false;
    }
}



function validateOTPDigits(inputElement,Count) {
    var regex = new RegExp("^\\d{" + Count + "}$");
    var input = inputElement.value;
    if (regex.test(input)) {
        return true;
    } else {
        alert("Please Enter 5 digit OTP Only");
        inputElement.value = '';
        return false;
    }
}
