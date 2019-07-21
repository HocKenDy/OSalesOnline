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
function ChuyenNgay(input_date) {
    if (input_date == '' || input_date == null) return '';
    var _date = input_date.split("-");
    var convertdate = "";
    if (_date != null) {
        if (_date.length > 1) {
            convertdate = _date[2].split('T')[0] + "/" + _date[1] + "/" + _date[0];
        }
    }
    else if (_date.length == 1) {
        convertdate = input_date.split(" ")[0];
    }
    return convertdate;
}

function GetDatatable(url, title) {
      var StartDate = $('#StartDate').val();  
 var EndDate = $('#EndDate').val();  
 var ProductInvoiceCode = $('#ProductInvoiceCode').val();  
 var Code = $('#Code').val();  
 		  ShowLoading();
        $.getJSON(url, {
        StartDate: StartDate,  
EndDate: EndDate,  
ProductInvoiceCode: ProductInvoiceCode,  
Code: Code
        }).done(function (data) {
            ReactDOM.render(
            <TableReport datatable={data} />,
        document.getElementById('react_report')
          );
		    HideLoading();
            });


      var TableReport = React.createClass({
 render: function() {
 
 sumQuantityPlan=0;
 sumAmount=0;
  sumQuantityErrorEstimates=0;
   sumQuantityEstimates=0;
 var tbody = this.props.datatable.map(function(comment_obj, i) {
   var convertFromDate = ChuyenNgay(comment_obj.FromDate);
      var convertToDate = ChuyenNgay(comment_obj.ToDate);
	    var convertDate = ChuyenNgay(comment_obj.Date);
 sumQuantityPlan += comment_obj.QuantityPlan;
 sumAmount += comment_obj.Amount;
 sumQuantityErrorEstimates+=comment_obj.QuantityErrorEstimates;
 sumQuantityEstimates+=comment_obj.QuantityEstimates;
   return (
  <BodyTable stt ={i + 1}
 
ProductInvoiceCode= { comment_obj.ProductInvoiceCode }
Code= { comment_obj.Code }
FromDate= { convertFromDate }
ToDate= { convertToDate }
QuantityPlan= { comment_obj.QuantityPlan }
QuantityErrorEstimates= { comment_obj.QuantityErrorEstimates }
QuantityEstimates= { comment_obj.QuantityEstimates }
ProductInvoiceId= { comment_obj.ProductInvoiceId }
ProdutName= { comment_obj.ProdutName }
ProdutCode= { comment_obj.ProdutCode }
Date= { convertDate }
Amount= { comment_obj.Amount }
PriceVon= { comment_obj.PriceVon }
   key ={i}>
    </BodyTable>
   );
    });
  return (
   <table className ="table table-bordered table-responsive" id = "cTable">
 <thead>
  <tr>
  <th colSpan = "11" className ="cell-center">< h4 >{ title} từ { StartDate} đến {EndDate}</h4></th>
  </tr>
  <tr>
 <th> STT </th>
 
 <th className="text-center">Mã đơn hàng</th>
 <th className="text-center">Mã KHSX</th>
 <th className="text-center">Ngày sản xuất</th>
 <th className="text-center">Tên sản phẩm</th>
 <th className="text-center">Mã sản phẩm</th>
  <th className="text-center">Chi phí</th>
  <th className="text-center">SL thành phẩm</th>
 <th className="text-center">SL lỗi</th>
 <th className="text-center">Tổng chi phí</th>
  </tr>
   </thead>
  <tbody>
  {tbody}
  <tr className="tr-bold">
  <td></td>
  
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
 <td className ="cell-center text-right"></td> 
<td className="text-right">{ConvertSoAm(sumQuantityEstimates)}</td>
<td className="text-right">{ConvertSoAm(sumQuantityErrorEstimates)}</td>
   <td className="text-right">{ConvertSoAm(sumAmount)}</td>
   </tr>
    </tbody>
    </table>
     );
     }
     });
     };

     
  var BodyTable = React.createClass({
   btnClickProductInvoice: function (ProductInvoiceId) {
            OpenPopup('/ProductInvoice/Detail/?Id=' + ProductInvoiceId + '&IsPopup=true', 'Chi tiết đơn hàng', 0, 500);
        },
    render: function() {
     var name = this.props.stt % 2 == 0 ? "alert-warning" : "";
    var color_QuantityErrorEstimates = parseInt(this.props.QuantityErrorEstimates) < 0 ? "text-right red":"text-right green"; 
  var color_Amount = parseInt(this.props.Amount) < 0 ? "text-right red":"text-right green"; 
   var color_PriceVon = parseInt(this.props.PriceVon) < 0 ? "text-right red":"text-right green"; 
      var color_QuantityEstimates = parseInt(this.props.QuantityEstimates) < 0 ? "text-right red":"text-right green"; 
  return (
   <tr className ={name}>
  <td>{this.props.stt}</td>
  <td><a onClick={() => this.btnClickProductInvoice(this.props.ProductInvoiceId)}>{this.props.ProductInvoiceCode}</a></td>
  <td>{this.props.Code}</td>
  <td>{this.props.Date}</td>
    <td>{this.props.ProdutName}</td>
  <td>{this.props.ProdutCode}</td>
     <td className ={color_PriceVon}>{ConvertSoAm(this.props.PriceVon)}</td>
	  <td className ={color_QuantityEstimates}>{ConvertSoAm(this.props.QuantityEstimates)}</td>
   <td className ={color_QuantityErrorEstimates}>{ConvertSoAm(this.props.QuantityErrorEstimates)}</td>
   <td className ={color_Amount}>{ConvertSoAm(this.props.Amount)}</td>
   </tr>
   );
   }
   });