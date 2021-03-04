function convertFirstLetterToUpperCase(text) {

    //charAt ile verdiğim indexteki harfi alıyorum.
    //slice ile verilen indexten itibaren verilen string ifade alınır.
    return text.charAt(0).toUpperCase() + text.slice(1);
}

function convertToShortDate(dateString) {
    const shortDate = new Date(dateString).toLocaleDateString('en-US');
    return shortDate;
}