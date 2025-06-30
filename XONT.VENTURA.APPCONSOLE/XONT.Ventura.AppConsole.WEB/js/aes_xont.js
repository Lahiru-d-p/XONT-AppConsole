function btnLogin_ClientClick() {
    console.log('here goes the values of pw');

    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

    var txtUserName = document.getElementById('txtUserName').value;
    var txtPassword = document.getElementById('txtPassword').value;

    $('#txtUserName').attr('disabled', 'disabled');
    $('#txtPassword').attr('disabled', 'disabled');

    var encryptedUN = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtUserName), key,
    {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    var encryptedPW = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtPassword), key,
   {
       keySize: 128 / 8,
       iv: iv,
       mode: CryptoJS.mode.CBC,
       padding: CryptoJS.pad.Pkcs7
   });

    document.getElementById('hdnEncryptUN').value = encryptedUN;
    document.getElementById('hdnEncryptPW').value = encryptedPW;
}