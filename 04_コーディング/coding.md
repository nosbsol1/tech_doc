# なぜ可読性・保守性が高いコードが必要なのか？　可読性・保守性を高める手法を考える

## なにこれ？
★TODO この記事で解決したいこと



ある程度コーディング経験がある方であれば、可読性や保守性の低いコードに悩まされた経験もあると思いますが、
新入社員の方には、可読性・保守性の重要さを直性肌で感じることは難しいかもしれません。
また、プログラミング言語や開発ツールの指導はあっても、コードの可読性・保守性がなぜ重要なのかを学ぶ機会はあまりないかと思います。

そこで、この記事では、コードの可読性や保守性がなぜ重要なのか、可読性・保守性を高める手法を紹介します。

## なぜ可読性・保守性が高いコードが必要なのか？
### ■可読性が低いコードは生産性が悪い

コードは、書かれる時間よりも読まれる時間の方が圧倒的に長いです。
通常、プログラム実装後は次のような工程があり、これらの全ての工程でコードが読まれることになります。

- コードレビュー
- テスト（テストコードを書く）
- バグの修正
- 機能改修
- 再利用（別プログラムへの流用）

コードレビューでは、レビューアーによってコードが読まれレビューされます。
テストコードを書く際にもコードは読まれます。
バグの修正をするには、コードを読んで原因の特定をする必要がありますし、
機能改修や別プログラムに流用する際も、コードを読んでどういった処理がされているのかを理解しないといけません。

これらの多くの「読む」という作業がある中で、読みにくいコードではかなりの時間を費やすことになり、生産性が悪くなります。



### ■保守性が低いコードは生産性が悪い

機能Aで使用している処理を機能Bでも使用することになり、
機能Aの処理を機能Bにコピペしたとします。
この時点では、処理を共通化するよりも一見早く実装できたように思えます。

ですが、後に何十もの機能に使用することになったとして
コピペした処理に修正が必要となった場合はどうでしょう。

共通化しておけば、修正は一か所で済んだかもしれませんが
コピペして複製してしまったがために、何十か所も修正が必要になってしまいます。

保守性の低いコードは、改修の影響が広範囲になり、修正箇所が増えます。
修正箇所が増えれば、バグが混入する確率も増え、品質の低下にもつながります。
品質を維持しようとすると、テストに多くの時間を費やさないといけなくなり、さらに生産性は悪くなります。


### ■可読性・保守性の低いコードがもたらす弊害
★TODO
- コードを読むのに時間がかかる
- バグが混入する確率が増える
- バグの原因特定にも時間がかかる
- 機能改修にも時間がかかる
- 開発者のモチベーションが下がる


## 可読性・保守性が低いコードの例と解決策
では、実際にどうすれば可読性・保守性を高められるのでしょうか。
可読性・保守性が低いコードの例から解決策を考えます。

### ■コードが長すぎる
長すぎるコードは読むのが大変です。
長すぎるコードは、分割してメソッドに抽出しましょう。

目安として、スクロールをしなくても全体が見える長さが読みやすいです。

<details>
<summary>
長すぎるメソッド
</summary>

```java
public boolean updRcd(SessionInfo pInfo, Session pSes, RetVal pRet, NxDcJyuHed pJyuHed,
            List<NxDcJyuDtl> pJyuDtlList, List<Boolean> pDelFlgList, List<Boolean> pHikRelFlgList,
            int pHisNbr, long pMotKurHedRcdId, long pMotAkaHedRcdId) {

        //受注見出しの元黒レコードの読み込み
        NxDcJyuHed myJyuHed = NxDcJyuHedDAO.getInst().getRcd_00(pSes, pMotKurHedRcdId);
        //受注見出しが正常に読み込めたか判断する
        if (myJyuHed == null) {
            pRet.addErr(Msg.getMsg("受注見出し", "BBLE0002", pInfo.getLang()), null);
            return false;
        }

        Transaction myTran = null;

        try {
            int myJyuLinNbr = 0;// 受注行№の宣言
            boolean myRes = false;// 結果
            long myMotKroDtlRcdId = 0;
            long myKroDtlRcdId = 0;

            NxDcJyuDtl myJyuDtl = null;
            NxDcJyuDtl myPrmJyuDtl = null;
            ZikMst myZikMstCls = new ZikMst();
            NxDcShnMstDAO myShnMstDAO = NxDcShnMstDAO.getInst();
            NxDcJyuDtlDAO myJyuDtlDAO = NxDcJyuDtlDAO.getInst();

            //倉庫コードマップを宣言
            //key：倉庫コード
            //value:変更区分　0：変更無し、1：変更有り
            Map<String, String> mySkoMap = new HashMap<String, String>();

            //項目変更フラグマップを宣言
            //key：明細レコードID
            //value:変更区分　0：変更無し、1：変更有り
            Map<String, String> myChgFlgMap = new HashMap<String, String>();

            if (pJyuDtlList.size() > 0) {
                // Listの要素がなくなるまで
                for (int i = 0; i < pJyuDtlList.size(); i++) {

                    myPrmJyuDtl = pJyuDtlList.get(i);
                    long myPrmJyuDtlRcdId = myPrmJyuDtl.getJyuDtlRcdId();
                    boolean myDelFlg = pDelFlgList.get(i);

                    // 更新区分を設定する
                    String myKsnKbn = "";// 更新区分
                    // 受注明細エンティティの受注明細レコードＩＤ = 0
                    if (myPrmJyuDtlRcdId == 0) {
                        myKsnKbn = "1";// 追加処理
                    }
                    // 取消フラグ (myDelFlg) = true
                    else if (myDelFlg) {
                        myKsnKbn = "3";// 取消処理
                    }
                    else {
                        myKsnKbn = "2";// 訂正処理
                    }

                    /*----- 現在レコードの読込み -----*/
                    String myStr1 = String.valueOf(myPrmJyuDtl.getJyuLinNbr());

                    // 更新区分 myksnKbn = 2 || myksnKbn = 3
                    if ("2".equals(myKsnKbn) || "3".equals(myKsnKbn)) {
                        myJyuDtl = myJyuDtlDAO.getRcd_00(pSes, myPrmJyuDtlRcdId);
                        // 受注明細が正常に読込めたかどうか判断する
                        if (myJyuDtl == null) {
                            pRet.addErr(Msg.getMsg(myStr1, "受注明細", "BBLE0005", pInfo.getLang()),
                                    null);
                            return false;
                        }
                    }
                    if (myKsnKbn.trim().equals("2")) {
                        //パラメータと現在レコードの受注明細項目が変更されているか判断する
                        if (!pJyuHed.getTokCd().trim().equals(myJyuHed.getTokCd().trim()) || //得意先コード
                                !pJyuHed.getSykCd().trim().equals(myJyuHed.getSykCd().trim()) || //出荷先コード
                                !pJyuHed.getEgyEmCd().trim().equals(myJyuHed.getEgyEmCd().trim()) || //営業担当コード
                                !myPrmJyuDtl.getSkoCd().trim().equals(myJyuDtl.getSkoCd().trim()) || //倉庫コード
                                !myPrmJyuDtl.getShnCd().trim().equals(myJyuDtl.getShnCd().trim()) || //商品コード
                                myPrmJyuDtl.getJyuSry() != myJyuDtl.getJyuSry() || //受注数量
                                !DateUtil.convertStringHifon(myPrmJyuDtl.getSykPlnDt()).equals(
                                        myJyuDtl.getSykPlnDt().toString()) || //出荷予定日
                                !DateUtil.convertStringHifon(myPrmJyuDtl.getNkiDt()).equals(
                                        myJyuDtl.getNkiDt().toString()) || //納期
                                !myPrmJyuDtl.getNkiStiKbn().trim().equals(
                                        myJyuDtl.getNkiStiKbn().trim())//納期指定区分
                        ) {
                            //倉庫コードが変更されているか判断する
                            if (!myPrmJyuDtl.getSkoCd().trim().equals(myJyuDtl.getSkoCd().trim())) {
                                //パラメータの倉庫コード、変更区分1
                                mySkoMap.put(myPrmJyuDtl.getSkoCd().trim(), "1");
                            }
                            //現在レコードの倉庫コード、変更区分1
                            mySkoMap.put(myJyuDtl.getSkoCd().trim(), "1");
                        }
                        else {
                            String myValue = mySkoMap.get(myPrmJyuDtl.getSkoCd().trim());
                            if (myValue == null || myValue.trim().length() == 0) {
                                //現在レコードの倉庫コード、変更区分0
                                mySkoMap.put(myJyuDtl.getSkoCd().trim(), "0");
                            }
                        }
                        //パラメータと現在レコードの受注明細項目が変更されているか判断する（製造指図出力区分更新用）
                        if (!pJyuHed.getTokCd().trim().equals(myJyuHed.getTokCd().trim())//得意先コード
                                || !pJyuHed.getSykCd().trim().equals(myJyuHed.getSykCd().trim())//出荷先コード
                                || !myPrmJyuDtl.getShnCd().trim()
                                        .equals(myJyuDtl.getShnCd().trim())//商品コード
                                || !myPrmJyuDtl.getShnNm().trim()
                                        .equals(myJyuDtl.getShnNm().trim())//商品名
                                || myPrmJyuDtl.getJyuSry() != myJyuDtl.getJyuSry()//受注数量
                                || !DateUtil.convertStringHifon(myPrmJyuDtl.getSykPlnDt()).equals(
                                        myJyuDtl.getSykPlnDt().toString())//出荷予定日
                                || !DateUtil.convertStringHifon(myPrmJyuDtl.getNkiDt()).equals(
                                        myJyuDtl.getNkiDt().toString())//納期
                                || !myPrmJyuDtl.getNkiStiKbn().trim().equals(
                                        myJyuDtl.getNkiStiKbn().trim())//納期指定区分
                                || !myPrmJyuDtl.getDtlBko().trim().equals(
                                        myJyuDtl.getDtlBko().trim())//明細摘要
                        ) {
                            myChgFlgMap.put(Converter.stringConvert(myPrmJyuDtlRcdId), "1");
                        }
                        else {
                            myChgFlgMap.put(Converter.stringConvert(myPrmJyuDtlRcdId), "0");
                        }
                    }
                    //取消
                    else if (myKsnKbn.trim().equals("3")) {
                        //現在レコードの倉庫コード、変更区分1
                        mySkoMap.put(myJyuDtl.getSkoCd().trim(), "1");
                    }
                    //追加
                    else {
                        //パラメータの倉庫コード、変更区分1
                        mySkoMap.put(myPrmJyuDtl.getSkoCd().trim(), "1");
                    }
                }
            }

            if (pJyuDtlList.size() > 0) {
                // Listの要素がなくなるまで
                for (int i = 0; i < pJyuDtlList.size(); i++) {
                    myPrmJyuDtl = pJyuDtlList.get(i);
                    long myPrmJyuDtlRcdId = myPrmJyuDtl.getJyuDtlRcdId();
                    boolean myDelFlg = pDelFlgList.get(i);
                    boolean myHikRelFlg = pHikRelFlgList.get(i);
                    String myPrmShnCd = myPrmJyuDtl.getShnCd();
                    String myPrmThiKbn = myPrmJyuDtl.getThiKbn();
                    Date myPrmSykPlnDt = myPrmJyuDtl.getSykPlnDt();

                    // 更新区分を設定する
                    String myKsnKbn = "";// 更新区分
                    // 受注明細エンティティの受注明細レコードＩＤ = 0
                    if (myPrmJyuDtlRcdId == 0) {
                        myKsnKbn = "1";// 追加処理
                    }
                    // 取消フラグ (myDelFlg) = true
                    else if (myDelFlg) {
                        myKsnKbn = "3";// 取消処理
                    }
                    else {
                        myKsnKbn = "2";// 訂正処理
                    }

                    //ワークの宣言
                    String mySizSszPrtKbn = "0";//製造指図書出力区分
                    String mySizSszNbr = null;//製造指図№

                    /*----- 現在レコードの読込み -----*/
                    String myStr1 = String.valueOf(myPrmJyuDtl.getJyuLinNbr());
                    double mySetHikSmiSry = 0d;// 引当済数量

                    // 更新区分 myksnKbn = 2 || myksnKbn = 3
                    if ("2".equals(myKsnKbn) || "3".equals(myKsnKbn)) {
                        myJyuDtl = myJyuDtlDAO.getRcd_00(pSes, myPrmJyuDtlRcdId);
                        // 受注明細が正常に読込めたかどうか判断する
                        if (myJyuDtl == null) {
                            pRet.addErr(Msg.getMsg(myStr1, "受注明細", "BBLE0005", pInfo.getLang()),
                                    null);
                            return false;
                        }

                        //現在レコードの値を保管
                        mySizSszPrtKbn = myJyuDtl.getSizSszPrtKbn();
                        mySizSszNbr = myJyuDtl.getSizSszNbr();
                    }

                    // 商品マスタの読込み
                    NxDcShnMst myShnMst = myShnMstDAO.getRcd_00(pSes, myPrmShnCd, myPrmSykPlnDt);
                    // 商品マスタが正常に読込めたかどうか判断する
                    if (myShnMst == null) {
                        pRet.addErr(Msg.getMsg(myStr1, "商品マスタ", "BBLE0005", pInfo.getLang()), null);
                        return false;
                    }
                    String myZikKnrKbn = myShnMst.getZikKnrKbn();

                    /*---------- トランザクションを開始する ----------*/
                    myTran = pSes.beginTransaction();

                    // 更新区分 (myksnKbn) = 1
                    if ("1".equals(myKsnKbn)) {
                        // 受注行№のカウントアップ
                        myJyuLinNbr += 1;

                        /*----- 受注明細登録 -----*/
                        // 受注明細エンティティ (myPrmJyuDtl) のセット
                        NxDcJyuDtl myJyuDtlInt = new NxDcJyuDtl();
                        myJyuDtlInt.setHisNbr(pHisNbr);// 履歴№
                        myJyuDtlInt.setJyuNbr(pJyuHed.getJyuNbr());// 受注№
                        myJyuDtlInt.setJyuHedRcdId(pJyuHed.getJyuHedRcdId());// 受注見出しレコードＩＤ
                        intEntJyuDtl(pInfo, myJyuDtlInt, myPrmJyuDtl, myShnMst, myJyuLinNbr,
                                mySetHikSmiSry);
                        // 受注明細エンティティを登録する
                        pSes.save(myJyuDtlInt);
                        pSes.flush();

                        myKroDtlRcdId = myJyuDtlInt.getJyuDtlRcdId();
                    }

                    /*----- 受注明細の訂正処理 -----*/
                    // 更新区分 (myksnKbn) = 2
                    if ("2".equals(myKsnKbn)) {
                        String myCrtDtTm = myJyuDtl.getCrtDtTm();
                        String myCrtEmCd = myJyuDtl.getCrtEmCd();
                        String myCrtPgm = myJyuDtl.getCrtPgm();
                        // 元黒レコード作成
                        pSes.evict(myJyuDtl);
                        // 受注明細エンティティ (myJyuDtl) のセット
                        myJyuDtl.setJyuDtlRcdId(0);
                        myJyuDtl.setRcdStsKbn("2");
                        setUpdTimeStamp(pInfo, myJyuDtl);
                        myJyuDtl.setJyuHedRcdId(pMotKurHedRcdId);

                        //先行手配かどうか判断する（無条件で９をセット）
                        myJyuDtl.setSizSszPrtKbn("9");//対象外

                        // 受注明細エンティティを登録する
                        pSes.save(myJyuDtl);
                        pSes.flush();
                        pSes.evict(myJyuDtl);

                        //変数宣言
                        myMotKroDtlRcdId = myJyuDtl.getJyuDtlRcdId();

                        // 元赤レコード作成
                        // 受注見出しエンティティ (myJyuDtl) のセット
                        intJyuDtl(pInfo, myJyuDtl, pHisNbr, pMotAkaHedRcdId);

                        //先行手配かどうか判断する
                        if (myJyuHed.getSzsKbn().trim().equals("1")) {
                            //元黒　製造指図書出力区分のセット 0：未出力
                            if (mySizSszPrtKbn.trim().equals("1")
                                    || mySizSszPrtKbn.trim().equals("2")
                                    || mySizSszPrtKbn.trim().equals("5")
                                    || mySizSszPrtKbn.trim().equals("6")) {
                                //倉庫コードが変更されているか判断する
                                if ((!myPrmJyuDtl.getSkoCd().trim().equals(
                                        myJyuDtl.getSkoCd().trim()))
                                        || (!myJyuDtl.getThiKbn().equals(HardCode.THI_KBN_CHOKUSO) && myPrmThiKbn.equals(HardCode.THI_KBN_CHOKUSO))
                                        || myZikKnrKbn.equals(HardCode.ZIK_KNR_KBN_NO)) {
                                    myJyuDtl.setSizSszPrtKbn("2");//取消
                                }
                                else {
                                    myJyuDtl.setSizSszPrtKbn("9");//出力済み
                                }
                            }
                            //出力済み
                            else {
                                myJyuDtl.setSizSszPrtKbn("9");//出力済み
                            }
                        }

                        // 受注明細エンティティを登録する
                        pSes.save(myJyuDtl);
                        pSes.flush();

                        /*----- 新黒レコード更新 -----*/
                        // 受注行№のカウントアップ
                        myJyuLinNbr += 1;

                        //現在レコード再読み込み
                        myJyuDtl = myJyuDtlDAO.getRcd_00(pSes, myPrmJyuDtlRcdId);

                        //先行手配かどうか判断する 1：先行手配
                        if (pJyuHed.getSzsKbn().trim().equals("1")) {
                            if (!myPrmJyuDtl.getSkoCd().trim().equals(myJyuDtl.getSkoCd().trim())) {
                                mySizSszPrtKbn = "0";//新規
                                mySizSszNbr = "";
                                myJyuDtl.setSizSszSmiSkoCd("");
                            }
                            else if (mySizSszPrtKbn.trim().equals("6")) {
                                mySizSszPrtKbn = "1";//変更
                            }
                            else {
                                if (mySizSszPrtKbn.trim().equals("2")
                                        || mySizSszPrtKbn.trim().equals("5")) {
                                    if (!"1".equals(myChgFlgMap.get(Converter
                                            .stringConvert(myPrmJyuDtlRcdId)))) {
                                        mySizSszPrtKbn = "2";//新規
                                    }
                                    else {
                                        mySizSszPrtKbn = "1";//変更
                                    }
                                }
                                else {
                                    mySizSszPrtKbn = myJyuDtl.getSizSszPrtKbn();
                                }
                            }
                        }
                        else {
                            mySizSszPrtKbn = "0";//未出力
                        }

                        /*----- 受注明細新黒更新 -----*/
                        // 受注明細エンティティ (myJyuDtl) のセット
                        myJyuDtl = updEntJyuDtl(pInfo, myJyuDtl, myPrmJyuDtl, myShnMst, pHisNbr,
                                myJyuLinNbr, mySetHikSmiSry);

                        myJyuDtl.setSizSszNbr(mySizSszNbr);
                        myJyuDtl.setSizSszPrtKbn(mySizSszPrtKbn);

                        myJyuDtl.setCrtDtTm(myCrtDtTm);
                        myJyuDtl.setCrtEmCd(myCrtEmCd);
                        myJyuDtl.setCrtPgm(myCrtPgm);
                        // 受注明細エンティティを更新する
                        pSes.update(myJyuDtl);
                        pSes.flush();
                    }

                    /*----- 受注明細の取消処理 -----*/
                    // 更新区分 (myksnKbn) = 3
                    if ("3".equals(myKsnKbn)) {
                        myMotKroDtlRcdId = myJyuDtl.getJyuDtlRcdId();

                        /*----- 元黒レコード論理削除 -----*/
                        // 受注明細エンティティ (myJyuDtl) のセット
                        myJyuDtl.setRcdStsKbn("2");
                        setUpdTimeStamp(pInfo, myJyuDtl);
                        myJyuDtl.setJyuHedRcdId(pMotKurHedRcdId);
                        myJyuDtl.setSizSszPrtKbn("9");//出力済み

                        // 受注明細エンティティを更新する
                        pSes.update(myJyuDtl);
                        pSes.flush();
                        pSes.evict(myJyuDtl);

                        /*----- 元赤レコード作成 -----*/
                        // 受注見出しエンティティ (myJyuDtl) のセット
                        intJyuDtl(pInfo, myJyuDtl, pHisNbr, pMotAkaHedRcdId);

                        //先行手配かどうか判断する 1：先行指図
                        if (myJyuHed.getSzsKbn().trim().equals("1")) {
                            //製造指図書出力区分のセット 0：未出力
                            if (mySizSszPrtKbn.trim().equals("0")
                                    || mySizSszPrtKbn.trim().equals("1")
                                    || mySizSszPrtKbn.trim().equals("2")) {
                                myJyuDtl.setSizSszPrtKbn("9");//出力済み
                            }
                            else {
                                myJyuDtl.setSizSszPrtKbn("2");//取消
                            }
                        }

                        // 受注明細エンティティを登録する
                        pSes.save(myJyuDtl);
                        pSes.flush();
                    }

                    /*---------- トランザクションをコミットする ----------*/
                    myTran.commit();

                    if ("1".equals(myKsnKbn)) {
                        myRes = myZikMstCls.updZikJyuSryForCrtJyuDtl(pInfo, pSes, pRet,
                                myKroDtlRcdId);

                        // 在庫マスタが正常に更新されたかどうか判断する
                        if (!myRes) {
                            return myRes;
                        }
                    }
                    if ("2".equals(myKsnKbn)) {
                        myRes = myZikMstCls.updZikJyuSryForUpdJyuDtl(pInfo, pSes, pRet,
                                myMotKroDtlRcdId, myJyuDtl.getJyuDtlRcdId(), myHikRelFlg);

                        // 在庫マスタが正常に更新されたかどうか判断する
                        if (!myRes) {
                            return myRes;
                        }
                    }
                    if ("3".equals(myKsnKbn)) {
                        myRes = myZikMstCls.updZikJyuSryForCanlJyuDtl(pInfo, pSes, pRet,
                                myMotKroDtlRcdId);

                        // 在庫マスタが正常に更新されたかどうか判断する
                        if (!myRes) {
                            return myRes;
                        }
                    }
                }
            }

            return true;
        }

        finally {
            if (myTran != null && myTran.isActive()) {
                myTran.rollback();
            }
        }
    }
```

</details>

### ■コードが重複している

### ■条件文が複雑
肯定形の方が理解しやすいです。
特に、否定の否定は混乱を招きやすいため、避けた方がよいです。
ド・モルガンの法則に沿って分かりやすく書き換えましょう。

悪い例
```java
if(!(a!=b && b!=c)) {
    ...
}
```
修正例
```java
if(a==b || b==c) {
    ...
}
```


### ■ネストが深すぎる
ネストが深くなると、コードが右にずれていき、物理的に読みにくくなります。
対象外の時はすぐに

悪い例
```java
public boolean checkUserCode(User user) {
    if(user != null) {
        if(user.code != null) {
            if(user.code.length() >= 3) {
                if(user.code.substring(0,2).equals("AB")) {
                    return true;
                }
            }
        }
    }  
    return false;
}
```
修正例
```java
public boolean checkUserCode(User user) {
    if(user == null) {
        return false;
    }
    if(user.code == null) {
        return false;
    }
    if(user.code.length() < 3) {
        return false;
    }
    if(user.code.substring(0,2).equals("AB")) {
        return true;
    }
    return false;
}
```


### ■変数名やメソッド名が適切でない

悪い例
```java
public int method(int a, int b, int c) {
    return a * b * c;
}
```
修正例
```java
public int calcCuboidVolume(int vertical, int horizontal, int height) {
    return vertical * horizontal * height;
}
```

```java
public BigDecimal method(BigDecimal a, BigDecimal b) {
		BigDecimal c;
		BigDecimal d = new BigDecimal("1.00");
		c = a.multiply(d.add(b));
		c = c.setScale(0, BigDecimal.ROUND_DOWN);
		return c;
	}
```
修正例
```java
public BigDecimal calcTax(BigDecimal kingaku, BigDecimal tax) {
		BigDecimal result;
		BigDecimal one = new BigDecimal("1.00");
		result = kingaku.multiply(one.add(tax));
		result = result.setScale(0, BigDecimal.ROUND_DOWN);
		return result;
	}
```
何をするメソッドなのか、何の値が入る変数なのかをイメージできる名前を付けましょう。

日本語を入力すると、自動でネーミングを作成してくれるツールもあります。
ネーミングに迷ったら、参考にしてみるといいかもしれません。

Codic
https://codic.jp/


### ■マジックナンバーが使用されている
マジックナンバーとは、意味のよく分からない数字のことです。

次の悪い例でいうと、「10」や「20」がマジックナンバーに該当します。
定数として定義して名前を付けてあげると、その数字の利用意図が分かりやすくなります。
また、共通化することで、数値に変更があった場合にも、修正箇所は最小限で済みます。

悪い例
```java
public BigDecimal calcTax(BigDecimal excludedPrice) {
    return excludedPrice.multiply(new BigDecimal(0.1));
}
```
```java
public void getUnit(int quantity) {
    if (quantity>=10 && quantity<=20) {
        ...
    }
}
```
修正例
```java
/** 消費税率 */
public static final BigDecimal TAX_RATE = new BigDecimal(0.1);

public BigDecimal calcTax(BigDecimal excludedPrice) {
    return excludedPrice.multiply(new BigDecimal(TAX_RATE));
}
```
```java
/** 最小数量 */
public static final int QUANTITY_MIN = 10;
/** 最大数量 */
public static final int QUANTITY_MAX = 20;

public void getUnit(int quantity) {
    if (quantity>=QUANTITY_MIN && quantity<=QUANTITY_MAX) {
        ...
    }
}
```

### 

## まとめ



