function ConvertSoAm(input_date) {
    var convertdate = "";
    if (parseInt(input_date) < 0) {
        var aa = parseInt(input_date) * (-1);
        var bb = aa.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        convertdate = "(" + bb + ")";
    }
    else {
        convertdate = parseInt(input_date).toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }
    return convertdate;
}
function GetDatatable(url, title) {
        var startdate = $('#start').val();
        var enddate = $('#end').val();

		 ShowLoading();
        $.getJSON(url, {
            StartDate: startdate,
            EndDate: enddate
        }).done(function (data) {
            ReactDOM.render(
            <TableReport datatable={data} />,
        document.getElementById('react_report')
          );
		   HideLoading();
            });


    //Create Table Report
    var TableReport = React.createClass({
        render: function () {
            var sumTongNoDauKy = 0;
            var sumTongNoCuoiKy = 0;
            var sumTangGiuaKy = 0;
            var sumGiamGiuaKy = 0;
            var tbody = this.props.datatable.map(function (comment_obj, i) {
                sumTongNoDauKy += comment_obj.TongNoDauKy;
                sumTongNoCuoiKy += comment_obj.TongNoCuoiKy;
                sumTangGiuaKy += comment_obj.TangGiuaKy;
                sumGiamGiuaKy += comment_obj.GiamGiuaKy;
                return (
                <BodyTable stt={i+1}
                           TargetCode={comment_obj.TargetCode}
                           TargetName={comment_obj.TargetName}
                           TongNoDauKy={comment_obj.TongNoDauKy}
                           TongNoCuoiKy={comment_obj.TongNoCuoiKy}
                           TangGiuaKy={comment_obj.TangGiuaKy}
                           GiamGiuaKy={comment_obj.GiamGiuaKy}
                           TongNoCuoiKy_Text={comment_obj.TongNoCuoiKy_Text}
                           TongNoDauKy_Text={comment_obj.TongNoDauKy_Text}
                           key={i}>
                </BodyTable>
    );
    });
    return (
    <table className="table table-bordered table-responsive" id="cTable">
        <thead>
            <tr>
                <th colSpan="8" className="cell-center" ><h4>{title} từ {startdate} đến {enddate}</h4></th>
            </tr>
            <tr>
                    <th rowSpan="2" className="text-center">STT</th>
                <th rowSpan="2" className="text-center">Mã khách hàng</th>
                <th rowSpan="2" className="text-center">Tên khách hàng</th>
                <th rowSpan="2" className="text-center">Tổng nợ đầu kỳ</th>
                <th colSpan="2" className="text-center">Trong kỳ</th>
                <th rowSpan="2" className="text-center">Tổng nợ cuối kỳ</th>
            </tr>
            <tr>
                    <th title="Tiền khách hàng nợ công ty" className="text-center">Ps Tăng</th>
                    <th title="Tiền công ty nợ khách hàng" className="text-center">Ps Giảm</th>
            </tr>
        </thead>
        <tbody>
            
            {tbody}

            <tr className="tr-bold">
                <td colSpan={3} className="cell-center">  <b>TỔNG CỘNG</b></td>
                <td className="text-right">{ConvertSoAm(sumTongNoDauKy)}</td>
                <td className="text-right">{sumTangGiuaKy.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className="text-right">{sumGiamGiuaKy.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className="text-right">{ConvertSoAm(sumTongNoCuoiKy)}</td>
             
            </tr>

        </tbody>
    </table>
    );
    }
    });
   
    //Create Table Report
    var BodyTable = React.createClass({
        btnClickTransactionLiabilities: function (event) {

            var start = $('#start').val();
            var end = $('#end').val();
            OpenPopup('/TransactionLiabilities/Report_LiabilitiesDetail/?StartDate=' + start + '&EndDate=' + end + '&TargetCode=' + event + '&SalerId=&strWarehouse=&TargetModule=Supplier&IsPopup=true', 'Chi tiết công nợ', 0, 500);
            },
        btnClick: function (event) {
            //alert("TargetCode"+event);
            //var startdate = $('#start').val();
            //var enddate = $('#end').val();
            //var TargetCode = event;
            OpenPopup('/Transaction/DanhSachMaHangChuaThanhToan/?CustomerCode=' + event + '&TargetType=Supplier&IsPopup=true', 'Chi tiết công nợ', 0, 500);
        },
        render: function () {
            var name = this.props.stt % 2 == 0 ? "alert- warning" : ""; 
            var TongNoDauKy = parseInt(this.props.TongNoDauKy) < 0 ? " text-right red" : "text-right green";
            //var TangGiuaKy = parseInt(this.props.TangGiuaKy) < 0 ? " text-right red" : "text-right green";
            var TongNoCuoiKy = parseInt(this.props.TongNoCuoiKy) < 0 ? "text-right red" : "text-right green";
           
            return (
                <tr className={name}>
                <td>{this.props.stt} </td>
                <td>{this.props.TargetCode} </td>
                <td><a onClick={() => this.btnClickTransactionLiabilities(this.props.TargetCode)}>{this.props.TargetName}</a></td>
                <td className={TongNoDauKy}>{this.props.TongNoDauKy_Text}</td>
                <td className="text-right">{this.props.TangGiuaKy.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className="text-right">{this.props.GiamGiuaKy.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")}</td>
                <td className={TongNoCuoiKy}><a onClick={() => this.btnClick(this.props.TargetCode)}>{this.props.TongNoCuoiKy_Text}</a></td>
            </tr>
            );
        }
    });
    };