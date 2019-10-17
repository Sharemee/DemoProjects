/**
* @license                                     
* 多级联动下拉框 for 牛发发
* ajax数据
* http://www.nffcms.com/
*
* Since: 2011/11/04
*/
(function ($) {
    $.DataAjaxSelect = {
        defaultval: 0, //起始默认值
        columns: []    //数据源列
    };
    $.AjaxSelect = function (p) {
        p = $.extend({}, $.DataAjaxSelect, p || {});
        var g = {
            //初始化
            Init: function () {
                g.select = [];
                for (var i = 0; i < p.columns.length; i++) {
                    var column = p.columns[i];
                    if (column) {
                        g.select[i] = column;
                        g.SelectInitBing(i, column);
                    }
                }
            },
            //动态获取数据
            AjaxLoadData: function (column, val) {
                if (column) {
                    if ((val && val.length && val.length > 0) || ($.type(val) == "number")) {
                        /*
                        val = val.toString();
                        if (val.lastIndexOf('_')) {
                        val = val.split("_")[0];
                        }
                        */
                        var url = column.url.replace(/(^\s*)|(\s*$)/g, "");
                        if (url.indexOf('?') < 0) {
                            url += "?";
                        }
                        else {
                            url += "&";
                        }
                        url += column.PKName + "=" + val;
                        $.ajax({
                            type: 'POST',
                            url: url,
                            data: {},
                            dataType: 'json',
                            success: function (data) {
                                g.SelectFill($(column.Obj), $.extend({}, data), column);
                            }
                        });
                    }
                    else {
                        g.SelectFill($(column.Obj), {}, column);
                    }
                }
            },
            //初始化绑定
            SelectInitBing: function (index, column) {
                var select = $(column.Obj);
                if (select.length > 0) {
                    var _Val;
                    if (index == 0) {
                        _Val = p.defaultval;
                    }
                    else {
                        _Val = g.select[index - 1].SelectVal;
                    }
                    if (!_Val) {
                        //如果为空，查找OnChang
                        for (var j = index - 1; j >= 0; j--) {
                            var _column = g.select[j];
                            if (_column) {
                                var _OnChang = _column.OnChang;
                                if (j + _OnChang == index) {
                                    if (_column.SelectVal) {
                                        _Val = _column.SelectVal;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    g.AjaxLoadData(column, _Val);
                    select.change(function () {
                        var val = $(this).val();
                        g.SelectBing(index + 1, column, val);
                    });
                }
            },
            //点击绑定数据
            SelectBing: function (index, column, val) {
                var nextcolumn = g.select[index];
                g.AjaxLoadData(nextcolumn, val);
                var OnChang = column.OnChang;
                for (var i = index + 1; i < g.select.length; i++) {
                    if (OnChang && i == (index - 1 + OnChang)) {
                        g.AjaxLoadData(g.select[i], val);
                    }
                    else {
                        var column = g.select[i];
                        if (column) {
                            this.SelectFill($(column.Obj), {}, column);
                        }
                    }
                }
            },
            //填充数据
            SelectFill: function (select, data, column) {
                select.empty();
                if ($.isArray(column.DefaultOpt)) {
                    for (var i = 0; i < column.DefaultOpt.length; i++) {
                        var row = column.DefaultOpt[i];
                        var defaultopt = $('<option value="' + row.val + '"  id="' + row.val + '">' + row.key + '</option>');
                        select.append(defaultopt);
                    }
                }
                else {
                    var defaultopt = $('<option value="' + column.DefaultOpt.val + '"  id="' + column.DefaultOpt.val + '">' + column.DefaultOpt.key + '</option>');
                    select.append(defaultopt);
                }
                for (var key in data) {
                    var row = data[key];
                    var arr = row.Text.toString().split("|");
                    var opts;
                    if (arr.length == 2) {
                        opts = $('<option value="' + row.Value + '"  id="' + arr[1] + '">' + arr[0] + '</option>');
                        //opts = $('<option value="' + row.Value + '">' + row.Text + '</option>');
                    }
                    else {
                        opts = $('<option value="' + row.Value + '"  id="' + row.Value + '">' + row.Text + '</option>');
                    }
                    //                    var selectinit = select.data("init");
                    //                    if (!selectinit && selectinit != row.Value && column && column.SelectVal == row.Value) {
                    //                        opts[0].selected = true;
                    //                        select.data("init", row.Value);
                    //                    }
                    if (column && column.SelectVal == row.Value) {
                        opts[0].selected = true;
                    }

                    select.append(opts);
                }
                if (column.combobox) {
                    select.ComboBox({ Refresh: true, autocomplate: column.autocomplate });
                }
            }
        };
        if (p.columns && p.columns.length > 0) {
            g.Init();
        }
        return g;
    };
})(jQuery);