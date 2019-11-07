//*************************
//說明:系統工具欄常用快捷鍵設置
//程序:Allen
//實例:
//**********************

document.writeln("<script type=\"text/javascript\" src=\"../Script/SysWin.js?ver=2.0\"></script>");
document.writeln("<script type=\"text/javascript\" src=\"../Script/RINDEX.js?ver=2.0\"></script>");

 //初始化
 document.onkeydown = PageKeyDown; 
 //document.onmousedown=PageMouseDown;
 
        
var getObj=function(id){ return 'string'==typeof(id)?document.getElementById(id):id;}
 
var userAgent = navigator.userAgent.toLowerCase();
var isSafari = userAgent.indexOf("Safari")>=0;
var is_opera = userAgent.indexOf('opera') != -1 && opera.version();
var is_moz = (navigator.product == 'Gecko') && userAgent.substr(userAgent.indexOf('firefox') + 8, 3);
var is_ie = (userAgent.indexOf('msie') != -1 && !is_opera) && userAgent.substr(userAgent.indexOf('msie') + 5, 3);
var ie  =navigator.appName=="Microsoft Internet Explorer"?true:false;

//鍵盤按下事件
function PageKeyDown(evt) {
    evt = evt || window.event;
    var keyChar = ie ? evt.keyCode : evt.which;

    if (evt.ctrlKey && keyChar == 83) { // 儲存:Ctrl+S
        if (getObj("td_save") != null)
            getObj("td_save").click();
        preventKeyCode(evt);
    } else if (evt.ctrlKey && keyChar == 78) { //新增:Ctrl+N
        if (getObj("td_new") != null)
            getObj("td_new").click();
        preventKeyCode(evt);
    } else if (evt.ctrlKey && keyChar == 81) { //查詢:Ctrl+Q
        if (getObj("td_query") != null)
            getObj("td_query").click();
        preventKeyCode(evt);
    }else if(evt.ctrlKey && keyChar == 90){ //取消: Ctrl+Z
        if (getObj("td_undo") != null)
            getObj("td_undo").click();
        preventKeyCode(evt);
    } else if (evt.ctrlKey && keyChar == 69) { //編輯:Ctrl+E
        if (getObj("td_edit") != null)
            getObj("td_edit").click();
        preventKeyCode(evt);
    }else if(evt.altKey && keyChar == 88){  //退出作業:Alt+X
        if (getObj("td_quit") != null)
            getObj("td_quit").click();
        preventKeyCode(evt);
    } else if (evt.ctrlKey && keyChar == 68) { //刪除:Ctrl+D
        if (getObj("td_delete") != null)
            getObj("td_delete").click();
        preventKeyCode(evt);
    } else if (keyChar == 112) { //查看幫助：F1
         if (getObj("td_helps") != null)
            getObj("td_helps").click();
        preventKeyCode(evt);
    }else  if (keyChar == 114) { //搜尋:F3
        if (getObj("td_search") != null)
            getObj("td_search").click();
        preventKeyCode(evt);
    }  else if (keyChar == 117) { //重設:F6
        if (getObj("td_reset") != null)
            getObj("td_reset").click();
        preventKeyCode(evt);
    } else if (keyChar == 118) { //重整:F7
        if (getObj("td_refurbish") != null)
            getObj("td_refurbish").click();
        preventKeyCode(evt);
    } else if (evt.keyCode == 121) { //審核:F10
        if (getObj("td_check") != null)
            getObj("td_check").click();
        preventKeyCode(evt);
    } else if (evt.shiftKey && keyChar == 121) {//反審:Shift+F10
        if (getObj("td_recheck") != null)
            getObj("td_recheck").click();
        preventKeyCode(evt);
    } else if (keyChar == 123) { //Excel:轉Excel
        if (getObj("td_excel") != null)
            getObj("td_excel").click();
        preventKeyCode(evt);
    } else if (evt.ctrlKey && event.keyCode == 80) { //列印:Ctrl+P
        if (getObj("td_print") != null)
            getObj("td_print").click();
        preventKeyCode(evt);
    } else if (keyChar == 116) {    //F5鍵彈編輯視窗
        var focusObj;
        //e=e?e:(window.event?window.event:null);
        var el = evt.srcElement ? evt.srcElement : evt.target;
        focusObj = document.activeElement;
        //當控件具有光標時也彈出框
        try {
            if (focusObj != 'undefined' && focusObj.id != "" &&
				 (focusObj.type == "text" && focusObj.maxLength >= 50 || focusObj.type == "textarea"))
                ShowF5Win(focusObj);
        } catch (e) { }
        preventKeyCode(evt);
    } else if (keyChar == 113) { //F2鍵開視窗
        var focusObj;
        var el = evt.srcElement ? evt.srcElement : evt.target;
        focusObj = document.activeElement;
        //當控件具有光標時也彈出框
        if (focusObj != 'undefined' && focusObj.id != "" && (focusObj.type == "text" || focusObj.type == "textarea")) {
            try { focusObj.ondblclick(); } catch (e) { }
        }
        preventKeyCode(evt);
    }  

}
//屏蔽Browser中快捷鍵
function preventKeyCode(evt) {
   evt = evt || window.event; 
   if (ie) { 
        evt.keyCode = 0;
        evt.returnValue = false;
        evt.cancelBubble = true;  
    } else {
        evt.preventDefault();
    }
}
//滑鼠按下事件
function PageMouseDown(e) {
    e = e ? e : (window.event ? window.event : null);
    var el = e.srcElement ? e.srcElement : e.target;
    var focusObj;
    if (ie) {
        focusObj = e.srcElement;
        // alert(e.srcElement);
        if (focusObj != undefined && focusObj.id != "" &&
            (focusObj.type == "text" && focusObj.maxLength >= 50
             || focusObj.type == "textarea")) {
            showF5Win(focusObj);
        }
    } else {
        focusObj = e.target;
        if (focusObj != undefined && focusObj.id != "" &&
				 (focusObj.type == "text" && focusObj.maxLength >= 50 || focusObj.type == "textarea")) showF5Win(focusObj);
        e.preventDefault(); //屏蔽Firefox默认处理
    }

}
	
//是否為非法日期
String.prototype.isDate= function(){ 
  var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})getObj/); 
   if(r==null)return false; var d = new Date(r[1], r[3]-1, r[4]); 
      return (d.getFullYear()==r[1]&&(d.getMonth()+1)==r[3]&&d.getDate()==r[4]); 
} 
 
//將Tab鍵變為回車鍵
//document.onkeydown = CheckKey;
function txtKeyDown(e){
  e=e?e:(window.event?window.event:null);
  var oText = ie? e.srcElement:e.target; 
  if(oText.type!="textarea"){
	  if(ie){
		if (e.keyCode==13) {
			e.keyCode=9;
			return;
		}
	  }else {
		  if(e.which==13){
			 e.which=9;
 			 return;
		  }
	  }
  }else{
    return true;
  }
 }
//用于文本框內容的選擇:是日期時選擇年份,否則全部選擇
function txtFocus(e)
{
    try {
        e = e ? e : (window.event ? window.event : null);
        var oText = ie ? e.srcElement : e.target;
        var strText = oText.value;
        if (strText.isDate()) {
            var r = strText.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})getObj/);
            var txtrange = oText.createTextRange();
            txtrange.findText(r[1]);
            txtrange.select();
        } else {
            oText.select();
        }
    } catch (e) { }
    
}
//滑鼠移到textbox事件
function txtOver(obj){
    var css = obj.className;
    var index1 = css.indexOf("txtStyle"); //一般文本框
    var index1_1= css.indexOf("txtOutStyle"); 
    var index2 = css.indexOf("txtnumStyle"); //數字輸入框
    var index2_1= css.indexOf("txtnumOutStyle"); 
    var index3 = css.indexOf("txtupperStyle");//大寫輸入框
    var index3_1= css.indexOf("txtupperOutStyle"); 
    
    if (index3!=-1||index3_1!=-1){
       obj.className='txtupperOverStyle';
    }else if(index2!=-1||index2_1!=-1){
       obj.className='txtnumOverStyle';
    }else {
       obj.className='txtOverStyle';
    }
}
//滑鼠離開textbox事件
function txtOut(obj){
    var css = obj.className;
    var index1 = css.indexOf("txtOverStyle"); //一般文本框
    var index2 = css.indexOf("txtnumOverStyle"); //數字輸入框
    var index3 = css.indexOf("txtupperOverStyle");//大寫輸入框
    if (index3!=-1){
       obj.className='txtupperOutStyle';
    }else if(index2!=-1){
       obj.className='txtnumOutStyle';
    }else{
       obj.className='txtOutStyle';
    }
 
}
//退出彈出窗體
function ExitWin(evt){
     evt=evt||window.event;
    var keyChar=evt.keyCode||evt.which;
   if(keyChar==27){
     window.close();
   }else if(keyChar==116){
      location.reload();
   }
}
   
/*鍵盤事件*/
function KeyDown(){ 
     if (window.event.keyCode == 13) //Enter
    {
        event.returnValue=false;
        event.cancel = true;
        //alert(event.srcElement.type);
        if (event.srcElement.type == 'text' || event.srcElement.type == 'select-one')
        {
            var i, oCurrentElement, txtElement;
            for (i=0;i<document.getElementsByTagName("a").length;i++)
            {
                oCurrentElement = document.getElementsByTagName("a").item(i);
                txtElement=oCurrentElement.innerText;
                if (txtElement == "确认")
                {
                    oCurrentElement.click();
                    return false;
                }
            }
            for (i=0;i<document.getElementsByTagName("input").length;i++)
            {
                oCurrentElement = document.getElementsByTagName("input").item(i);
                txtElement=oCurrentElement.value;
                if (txtElement == "刷新")
                {
                    oCurrentElement.click();
                    return false;
                }
            }
        }
	    return false;
    }
    else if (window.event.keyCode == 27)    //ESC
    {
        event.returnValue=false;
        event.cancel = true;
        if (event.srcElement.type == 'text' || event.srcElement.type == 'select-one')
        {
            var i, oCurrentElement, txtElement;
            for (i=0;i<document.getElementsByTagName("a").length;i++)
            {
                oCurrentElement = document.getElementsByTagName("a").item(i);
                txtElement=oCurrentElement.innerText;
                if (txtElement == "取消")
                {
                    oCurrentElement.click();
                    break;
                }
            }
        }
	    return false;
    }
    else if (window.event.keyCode == 8)    //Back Space
    {
        if (event.srcElement.type != 'text' && event.srcElement.type != 'password')
        {
            event.returnValue=false;
            event.cancel = true;
	        return false;
	    }
    }

}

function dropKeyUp(){ 
    if (window.event.keyCode == 13)
    {
        event.returnValue=false;
        event.cancel = true;

        var i, oCurrentElement, txtElement;
        for (i=0;i<document.getElementsByTagName("a").length;i++)
        {
            oCurrentElement = document.getElementsByTagName("a").item(i);
            txtElement=oCurrentElement.innerText;
            if (txtElement == "查找")
            {
                oCurrentElement.click();
                break;
            }
        }
        //MD，太诡异了，如果没有这一句，那么焦点就跑到IE的菜单栏去，郁闷。
        oCurrentElement.focus();
	    return false;
    }
}

var lastPressTime = 0;
var strLookUp = "";
function catch_press(oSel) {
    if (event.keyCode < 0x2f)
        return;

    //sel.options[sel.selectedIndex].text = sel.options[sel.selectedIndex].text + String.fromCharCode(event.keyCode); 
    var thisPressTime = new Date().getTime();
    var ln = oSel.options.length;

    if ((thisPressTime - lastPressTime) > 2000)   //两次按键的间隔小于2秒，或者第一次按键
        strLookUp = "";
    strLookUp = strLookUp + String.fromCharCode(event.keyCode);
    var utext = strLookUp.toUpperCase();
    //document.getElementById("ctl00_contentMain_Label2").innerText = utext;
    for (var i = 0; i < ln; i++) {
        var newtxt = oSel.options[i].text;
        var uopt = newtxt.toUpperCase();
        if (uopt != utext && 0 == uopt.indexOf(utext)) {
            //var txtrange = event.srcElement.createTextRange();
            //event.srcElement.value = strLookUp + newtxt.substr(strLookUp.length);
            //var txtrange = document.body.createTextRange();
            //txtrange.moveToElementText(oSel);
            //txtrange.moveStart("character", strLookUp.length);
            //txtrange.select();
            oSel.selectedIndex = i;
            break;
        } else if (uopt == utext) {
            oSel.selectedIndex = i;
        }
    }
    lastPressTime = thisPressTime;
    event.returnValue = false;
}




/*查找功能
*onclick="return   findInPage('2');"
*/
 //查找功能
  var   NS4   =   (document.layers);       
  var   IE4   =   (document.all);   
  var   win   =   window;         
  var   n       =   0;
  function findInPage(str) {
      var txt, i, found;
      if (str == "") return false;
      if (NS4) {
          if (!win.find(str))
              while (win.find(str, false, true))
              n++;
          else
              n++;
          if (n == 0)
              alert("Not   found.");
      }
      if (IE4) {
          txt = win.document.body.createTextRange();
          for (i = 0; i <= n && (found = txt.findText(str)) != false; i++) {
              txt.moveStart("character", 1);
              txt.moveEnd("textedit");
          }
          if (found) {
              txt.moveStart("character", -1);
              txt.findText(str);
              txt.select();
              txt.scrollIntoView();
              n++;
          }
          else {
              if (n > 0) {
                  n = 0;
                  findInPage(str);
              }
              else
                  alert("没有找到匹配内容！");
          }
      }
      return false;
  }   
  
 //短時間內禁用重覆點擊
 var limitClick = "";
 function limitTime() {
     if (limitClick == "load") {
         alert("請勿重覆點擊，請確認!!");
         return false;
     } else {
         window.setTimeout("limitClick='';", 2000);
         limitClick = "load";
         return true;
     }
 }
 //=========================創建HashTable=======================================================
 //創建一個Hashtable類
HashTable=function(){
  this.__hash = {}; //聲明一個鍵值對容器
}
//Hashtable類 屬性
HashTable.prototype = {
    Add: function(key, value) {
        if (typeof (key) != "undefined") {
            //if it not contains in hashtable 
            if (!this.contains(key)) {
                this.__hash[key] = typeof (value) == "undefined" ? null : value;
                return true;
            } else { return false; }
        }
    },

    Remove: function(key) { delete this.__hash[key]; },

    count: function() {
        var i = 0;
        for (var obj in this.__hash) {
            i++;
        }
        return i;
    },

    items: function(key) { return this.__hash[key]; },

    contains: function(key) { return typeof (this.__hash[key]) != "undefined"; },

    clear: function() {
        for (var obj in this.__hash) {
            delete this.__hash[k];
        }
    }

};
//============================不規則表格行單擊====================================================
// 功能: 不規則表格行單擊
// 說明: 此事件適用于Repeater行單擊
// 調用：
//       <ItemTemplate>
//       <span id='tr<%#  Container.ItemIndex   +   1 %>' onclick="Test(this,'<%# Eval("gkey") %>')" >
//          <tr>.................
//       </span>
//      </ItemTemplate>
// 程序: Allen Wang
// 日期: 2010-03-10 16:02
// 版本: V3.0
//==================================================================================================
var selectColor = "#BAF3A2";
var SelRowsHashTable = new HashTable();
 /**
 * 單擊行改變背景色 ，并傳遞參數
 * @method focusRow2
 * @param obj 當前span對像
 * @param gkey 主鍵
 * @return void 
 */
function focusRow2(obj, gkey) {
   if (obj == undefined) return;
 	var event = window.event;
 	var parentTable = obj.parentNode.parentNode; //取得表對像
 	if (parentTable.tagName.toLowerCase() != "table")
 		parentTable = parentTable.parentNode;
 	var parentTableID = parentTable.id;
 	
 	//單選
	var selectedRows = SelRowsHashTable.items(parentTableID);
 
	clearSelRowStyle(selectedRows);

	SelRowsHashTable.Remove(parentTableID);

	setChildRowColor(obj, selectColor); //設置Span下所有行的背景色
	if (selectedRows == null) selectedRows = new Array();
 	selectedRows.push(obj);
 	SelRowsHashTable.Add(parentTableID, selectedRows);
}
//改變span下所有行的背景色
function setChildRowColor(obj, color) {
    if (obj == undefined) return;
	var Elements = obj.childNodes;
    for (var i = 0; i < Elements.length; i++) {
 		var row = Elements[i];
 		if (row.tagName.toLowerCase() == "tr") {
 			row.style.backgroundColor = color;
 		}
 	}
 }
 /**
  * 清除選擇行的背景色
  * @method ClearSelectStyle
  * @param selectedRows{Array:HTMLSpanElement} 選擇行陣列
  * @return void 
  */ 
 function clearSelRowStyle(selectedRows){
     if(selectedRows==null) return;
      for(var i=0;i<selectedRows.length;i++){
         setChildRowColor(selectedRows[i], ''); //為Span對像
     } 
	selectedRows.length=0;//清空CSS樣式后，也清空容器
}
/**
 * 遍歷Table,清除選擇行樣式
 * @method clearSelRowStyle2
 * @param tblName 表名稱
 * @return void 
 */
function clearSelRowStyle2(tblName) {
    var Table = typeof (tblName) == "string" ? document.getElementById(tblName) : tblName;
    for (var i = 1; i < Table.rows.length; i++) {
        var tmpRow = Table.rows[i];
        if (tmpRow.style.backgroundColor.toLowerCase() == selectColor.toLowerCase() ||
	       tmpRow.style.backgroundColor.toLowerCase().indexOf("rgb(186, 243, 162)") != -1) {
            tmpRow.style.backgroundColor = "";
        }
    }
}

 /**
 * 加選擇行加入到緩存中
 * @method addSelRow
 * @param tblName{string} 表名
 * @param row{HTMLSpanElement} 選擇行所在span對像
 * @return void 
 */
function addSelRow(tableID,spanObj){
   var tableName=typeof(tableID)=="string"?tableID:tableID.getAttribute("id");
   var selectedRows=SelRowsHashTable.items(tableName);
   if(selectedRows==null)
 	   selectedRows=new Array();
   selectedRows.push(spanObj);
   SelRowsHashTable.Add(tableName,selectedRows); //寫入緩存
}

//====================特殊欄位控制============================================
// 存放Repeat中隱藏，唯讀欄位DBName, 格式: key:tableid,value:RepeatCol對像
var RepeatColHashTable = new HashTable();
 
RepeatCol = function(noShowItemDBName, noEditItemDBName) {
    this.NoShowItemDBNameArry = new Array();
    if (noShowItemDBName)
        this.NoShowItemDBNameArry = noShowItemDBName.split(",");
    this.NoEditItemDBNameArry = new Array();
    if (noEditItemDBName)
        this.NoEditItemDBNameArry = noEditItemDBName.split(",");
} 