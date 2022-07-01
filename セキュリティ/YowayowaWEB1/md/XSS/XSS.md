# ２．XSS（クロスサイト・スクリプティング）

## ■一言でいうと

Webアプリケーションにスクリプトを埋め込めてしまい、他の利用者のブラウザ上で不正なスクリプトが実行されてしまうこと。

## ■発生しうる脅威

* 本物サイト上に偽のページが表示される。  
  フィッシング詐欺に利用される。  

* ブラウザが保持している情報（CoookieやLocal Storageなど）の流出  
   なりすましやセッションハイジャックの被害にあう。

  など

## ■ 対策  

* 要素のエスケープ処理（サニタイジング）  

  Webページを構成する要素（<、>、&など）をHTMLエンティティ（\&lt;、\&gt;、\&amp;など）に置換する。  
  対象は入力値や外部取込みしたファイルに含まれる文字列、その他演算によって生成される文字列

* CookieにHttpOnly属性を加える  

  javascriptからCookieへのアクセスを禁止することで、Cookie漏洩対策を行う。  
  XSSの脅威は残る。。

## ■ デモ    

### 例1）本物サイト上に偽のページが表示される。 

最初に掲示板サイトが表示されますが、攻撃者が偽のページ（スクリプト）を埋め込むことで
掲示板が表示されず、個人情報の入力を促すフィッシング詐欺用の画面が表示されるようになりました。

一見URLも正しく、作りこまれた画面であればなかなかフィッシングに気づきにくいかもしれません。
また、自分で掲示板を開こうとした場合は個人情報入力画面が表示されることに違和感がありますが、
メールで直接リンクが送られてきた場合には一層分かりにくいと思います。

<img src="contents/1_偽ページ表示.gif" width="80%">
<div style="width:80%;text-align:center">画像1.偽ページ表示</div>


もちろんiframeを使えば予め用意しておいたサイトを表示することも可能です。
````
<script> var objXssPanel = document.getElementById('xss_panel'); objXssPanel.style.display='none'; var frameCreate = document.createElement('iframe'); frameCreate.src = 'https://www.nos.co.jp/'; frameCreate.style='border:none;width:100%;height:100vh;padding:0;margin:0;'; document.body.append(frameCreate); </script>
````

<img src="contents/2_偽ページiframe.gif" width="80%">
<div style="width:80%;text-align:center">画像2.偽ページ表示</div>

### 例2）Coookieの流出

Cookieがjavascriptからアクセスできてしまうと、Cookie情報を送信するスクリプトを埋め込まれた場合に送信されてしまいます。  
例えば画像のように攻撃者がCookieを攻撃者宛てに送信するリンク（スクリプト）を埋め込んだ後に、  
被害者が埋め込まれたリンクをクリックするとセッション等のCookie情報が送信されます。  

<img src="contents/3_Cookie流出.gif" width="80%">
<div style="width:80%;text-align:center">画像3.Cookie流出</div>

### 例3）サニタイジング  

氏名の表示はサニタイジングした結果を表示しています。   
例えば先ほどのCookie送信スクリプトを氏名欄に埋め込むと、  
ブラウザでスクリプトとして認識されず、文字列として認識されるようになります。  

※開発者ツールでHTMLを確認するとカッコ等の文字がエスケープされていることが確認できます。  

<img src="contents/4_サニタイジング.gif" width="100%">
<div style="width:100%;text-align:center">画像4.サニタイジング</div>




