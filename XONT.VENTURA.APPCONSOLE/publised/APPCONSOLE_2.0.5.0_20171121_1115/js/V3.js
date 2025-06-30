var theme_color='blue';
var link=window.parent.document.getElementsByTagName('link');
            
if(link){
    for(var i=0;i<link.length;i++){
        var href =link[i].href;
        if(href.includes('App_Themes') && href.includes('StyleSheet')){
            var array=href.split('App_Themes/');
            var new_color=array[1].split('/')[0];
            if(new_color && new_color!=''){
                theme_color=new_color;
            }
        }
    }
}
            
document.write('<link href="../App_Themes/' + theme_color + '/V3.css" rel="stylesheet" />');

function closeTab() {
    var t = window.parent.document.getElementById('txtCurrentTaskCode');
    if (t != null) {//check this one working in app console or local
        window.parent.document.getElementById(t.value + '_close').click();
    } else {
        var taskCode = currentTaskCode();
        cleanLocalStorage(taskCode);
    }
}

function currentTaskCode() {
    var base = document.getElementsByTagName('base');
    var taskCode = '';
    if(base){
        var href = base[0].href;
        var array = href.split('/');
        taskCode= array[array.length-2];
    }
    return taskCode.trim();
}

function cleanLocalStorage(taskCode) {
    for (var key in localStorage) {
        var keySplitArray = key.split('_');
        if (keySplitArray.length>0) {
            if (keySplitArray[0]==taskCode) {
                localStorage.removeItem(key);
            }
        }
    }
}
