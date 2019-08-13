
function iFrameOn() {
    richTextField.document.designMode = "On";
}
function iBold() {
    richTextField.document.execCommand('bold', false, null);
}
function iUnderline() {
    richTextField.document.execCommand('underline', false, null);
}
function iItalic() {
    richTextField.document.execCommand('italic', false, null);
}
function iFontSize() {
    var size = prompt('Enter a size 1 - 7', '');
    richTextField.document.execCommand('FontSize', false, size);
}
function iForeColor() {
    var color = prompt('Type a basic color or hexadecimal you wont to aplly:', '');
    richTextField.document.execCommand('ForeColor', false, color);
}
function iHorizontalRule() {
    richTextField.document.execCommand('inserthorizontalrule', false, null);
}
function iUnorderedList() {
    richTextField.document.execCommand('insertunorderedlist', false, "newUL");
}
function iOrderedList() {
    richTextField.document.execCommand('insertorderedlist', false, "newOL");
}
function iLink() {
    var linkURL = prompt("Insert a link:", "http://");
    richTextField.document.execCommand('Createlink', false, linkURL);
}
function iUnlink() {
    richTextField.document.execCommand('Unlink', false, null);
}
function iImage() {
    var imgSrc = prompt('Insert a image link/location:', '');
    if (imgSrc != null) {
        richTextField.document.execCommand('insertimage', false, imgSrc);
    }
}
function iCodeView() {
    var html = prompt('HTML?');
    richTextfield.document.execCommand('inserthtml', false, html);
}

var oDoc, sDefTxt;

function initDoc() {
    oDoc = document.getElementsByName("textBox-rtf")[0];
    sDefTxt = oDoc.innerHTML;
    if (document.compForm.switchMode.checked) { setDocMode(true); }
}

function formatDoc(sCmd, sValue) {
    if (validateMode()) { document.execCommand(sCmd, false, sValue); oDoc.focus(); }
}
function validateMode() {
    if (!document.compForm.switchMode.checked) {

        return true;

    }


    alert("Uncheck \"Show HTML\".");
    oDoc.focus();
    return false;
}


function setDocMode(bToSource) {
    var oContent;
    if (bToSource) {
        oContent = document.createTextNode(oDoc.innerHTML);
        oDoc.innerHTML = "";
        var oPre = document.createElement("pre");
        oDoc.contentEditable = false;
        oPre.id = "sourceText";
        oPre.contentEditable = true;
        oPre.appendChild(oContent);
        oDoc.appendChild(oPre);
    } else {
        if (document.all) {
            oDoc.innerHTML = oDoc.innerText;
        } else {
            oContent = document.createRange();
            oContent.selectNodeContents(oDoc.firstChild);
            oDoc.innerHTML = oContent.toString();
        }
        oDoc.contentEditable = true;
    }
    oDoc.focus();
}

function printDoc() {
    if (!validateMode()) { return; }
    var oPrntWin = window.open("", "_blank", "width=450,height=470,left=400,top=100,menubar=yes,toolbar=no,location=no,scrollbars=yes");
    oPrntWin.document.open();
    oPrntWin.document.write("<!doctype html><html><head><title>Print<\/title><\/head><body onload=\"print();\">" + oDoc.innerHTML + "<\/body><\/html>");
    oPrntWin.document.close();
}
