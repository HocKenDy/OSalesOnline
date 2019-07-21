/***
* Combojax
*/
window.pageCombojaxs = window.pageCombojaxs || {};
$.fn.extend({
    combojax: function (params) {
        var aObj = [];
        $(this).each(function () {
            if (!$(this).data("combojax")) {
                var combojax = new ComboJax($(this), params);
                var name = $(this).attr("id");
                if (name.length > 0)
                    window.pageCombojaxs[name] = combojax;

                aObj.push(combojax);
                $(this).data("combojax", combojax);
            } else {
                aObj.push($(this).data("combojax"));
            }
        });
        if (aObj.length == 1)
            return aObj[0];
        return aObj;
    }
});

ComboJax = (function ($) {
    function comboJax(input, options) {        
        this.input = input;
        this.input.attr("data-id", "");
        this.id = 'combojax_' + input.attr("id");
        input.parent().append('<div class="combojax"></div>');
        input.parent().find('div').attr('id', this.id);
        this.container = $("#" + this.id);
        input.appendTo(this.container);
        this.container.append('<ul></ul>');
        this.ul = $("#" + this.id + ' ul');
        this.data = [];

        options = options || {};
        this.options = $.extend({}, this.defaults(), options);
        this.init();
    }

    /**** init ************************/
    comboJax.prototype.init = function () {
        var $this = this;
        $this.ulShowHide = false;

        $.getJSON($this.options.url, {}, function (response) {
            $(response).each(function (index, obj) {
                var item = "<li class='item' data-id='" + obj.Id + "'>" + obj.Name + "</li>";
                $this.ul.append(item);
                $this.data.push(obj);
            });
        });

        //------- input ------------------------------------
        $this.input.bind('click', function (event) {
            //console.log("combojax click");
            event.stopPropagation();
        });

        $this.input.bind('focus', function (event) {
            console.log("combojax focus");
            $(".combojax ul").hide();
            search($this.input.val());
            $this.input.select();
        });

        $this.input.bind('blur', function (event) {
            console.log("combojax blur");
            combojaxSelectItem($this.input.val());
        });

        var timeout = null;
        $this.input.keydown(function (e) {
            if (e.which == 37 || e.which == 39) {
                e.preventDefault();
            }
            else {
                if (e.which == 9) {
                    combojaxSelectItem($this.input.val());
                }
                else {
                    if (e.which == 40) {
                        var selectedItem = $this.ul.find(".item.selected");
                        if (!selectedItem.is(':last-child')) {
                            selectedItem.removeClass("selected").next().addClass("selected");
                            var name = selectedItem.next().text();
                            var id = selectedItem.next().data("id");
                            //combojaxSelectItem(name);
                            $this.input.val(name);
                            $this.ul.scrollTop($this.ul.scrollTop() + selectedItem.next().position().top);
                        }
                    }
                    else if (e.which == 38) {
                        var selectedItem = $this.ul.find(".item.selected");
                        if (!selectedItem.is(':first-child')) {
                            selectedItem.removeClass("selected").prev().addClass("selected");
                            var name = selectedItem.prev().text();
                            var id = selectedItem.prev().data("id");
                            //combojaxSelectItem(name);
                            $this.input.val(name);
                            $this.ul.scrollTop($this.ul.scrollTop() + selectedItem.prev().position().top);
                        }
                    }
                    else if (e.which == 13) {
                        var selectedItem = $this.ul.find(".item.selected");
                        var id = selectedItem.data("id");
                        var name = selectedItem.text();

                        //Gọi callback khi không tìm thấy kết quả
                        if (name == '' && $this.options.onNotFound != undefined)
                        {
                            $this.options.onNotFound($this.input.val());
                        }

                        combojaxSelectItem(name);

                        showList(!$this.ulShowHide);
                    }
                    else {
                        if ($this.ulShowHide == false) {
                            showList(true);
                        }
                    }
                }
            }
        });

        $this.input.keyup(function (e) {
            if (e.which == 13 || e.which == 37 || e.which == 38 || e.which == 39 || e.which == 40)
                return;

            var searchText = $(this).val().trim();
            //if (searchText == '') {
            //    $this.input.data("id", "");
            //    $this.input.trigger("change");
            //}

            //if you already have a timout, clear it
            if (timeout) { clearTimeout(timeout); }

            //start new time, to perform ajax stuff in 500ms
            timeout = setTimeout(function () {
                search(searchText);
            }, 0);
        });

        //Sự kiện khi bấm ra ngoài thì ẩn khung tìm kiếm
        $(window).click(function (event) {
            $this.ul.hide();
        });

        //--------- function -------------------------------
        var combojaxSelectItem = function combojaxSelectItem(name) {
            console.log("combojaxSelectItem: " + name);

            var found = false;
            var obj = null;
            for (var i in $this.data) {
                if ($this.data[i].Name == name) {
                    found = true;
                    obj = $this.data[i];
                    $this.input.val(obj.Name);
                    $this.input.data("id", obj.Id);
                    $this.input.data("value", obj.Value);
                    break;
                }
            }

            if (!found) {
                $this.input.val('');
                $this.input.data("id", "");
                $this.input.data("value", "");
            }

            $this.input.trigger("change");
            if ($this.options.onSelected != undefined) {
                $this.options.onSelected(obj);
            }
        }

        function showList(show) {
            if (show) {
                $this.ul.show();
                $this.ulShowHide = true;
            }
            else {
                $this.ul.hide();
                $this.ulShowHide = false;
            }
        }

        function search(searchText) {
            $this.ul.html("");
            // Toán tử số học (Arithmetic operators), Toán tử quan hệ (Relational operator), Toán tử logic (Logical operator), Toán tử điều kiện (Condition operator), Các toán tử tăng, giảm (Increment and decrement operator), Toán tử gán (Assignment operator)
            var relational_operator = '>,>=,<,<=,=';
            var isSearchNumber = false;

            //lấy từ đầu tiên (tính bằng khoảng trắng) trong nội dung tìm kiếm, xem có các toán tử so sánh hay không để tìm kiếm cho giá trị số
            var first_word = searchText.split(' ')[0].trim().replace(/\d/g, '');
            if (relational_operator.indexOf(first_word) != -1) {
                isSearchNumber = true;

                // nếu tìm kiếm cho số thì bỏ đi các toán tử so sánh, chỉ giữ lại số
                searchText = searchText.replace(/\D/g, '');
            }

            searchText = convertVNtoEN(searchText);
            var result = [];
            if (searchText != '') {
                var numberOfItem = 0;
                for (var i in $this.data) {
                    var obj = $this.data[i];
                    if (numberOfItem <= 10) {
                        if (convertVNtoEN(obj.Name).indexOf(searchText) != -1) {
                            result.push(obj);
                            numberOfItem++;
                        }
                    }
                }
            }

            if (result.length > 0) {
                for (i = 0; i < result.length; i++) {
                    //var item = "<li class='item' data-id='" + result[i].Id + "' onclick='combojaxSelectItem(\""+result[i].Name + "\")'>" + result[i].Name + "</li>";
                    var item = "<li class='item' data-id='" + result[i].Id + "'>" + result[i].Name + "</li>";
                    $this.ul.append(item);
                }

                showList(true);

                $this.ul.find(".item:first").addClass("selected");

                $this.ul.find('li').bind('click', function (event) {
                    console.log("combojax click in ul");
                    combojaxSelectItem($(this).text());
                    showList(false);
                });
            }
        }
    };

    comboJax.prototype.defaults = function () {
        return {
            url: ''
        };
    };

    comboJax.prototype.addItem = function (obj) {
        console.log("comboJax.prototype.addItem", obj);
        var $this = this;
        $this.data.push(obj);
        $this.input.val(obj.Name);
        $this.input.focus();
    };

    return comboJax;
})(window.jQuery);