'use strict';
$(function () {
    GetDashboardData();
    // desktopChart1();
    //desktopChart2();
    initBedgeChart();
    chart1();
    chart2();
});
//Charts
function desktopChart1(months, salProcessed, salFailed) {
    /* Shadow */
    var draw = Chart.controllers.line.prototype.draw;
    Chart.controllers.line = Chart.controllers.line.extend({
        draw: function () {
            draw.apply(this, arguments);
            var ctx = this.chart.chart.ctx;
            var originalStroke = ctx.stroke
            ctx.stroke = function () {
                ctx.save();
                ctx.shadowColor = '#000';
                ctx.shadowBlur = 20;
                ctx.shadowOffsetX = 8;
                ctx.shadowOffsetY = 25;
                originalStroke.apply(this, arguments)
                ctx.restore();
            };
        }
    });

    var ctx = document.getElementById("desktop-chart1");

    ctx.height = 300;
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: months,
            type: 'line',
            datasets: [{
                label: "Paid",
                data: salProcessed,
                backgroundColor: 'transparent',
                borderColor: '#71D875',
                borderWidth: 3,
                pointStyle: 'circle',
                pointRadius: 5,
                pointBorderColor: '#71D875',
                pointBackgroundColor: '#fff'

            }, {
                label: "Un Paid",
                data: salFailed,
                backgroundColor: 'transparent',
                borderColor: "#F68A60",
                borderWidth: 3,
                pointStyle: 'circle',
                pointRadius: 5,
                pointBorderColor: '#F68A60',
                pointBackgroundColor: '#fff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            tooltips: {
                mode: 'index',
                titleFontSize: 12,
                titleFontColor: '#fff',
                bodyFontColor: '#fff',
                backgroundColor: '#929090',
                cornerRadius: 10,
                intersect: false,
            },
            legend: {
                display: true,
                labels: {
                    usePointStyle: true,
                },
            },
            scales: {
                xAxes: [{
                    display: true,
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    scaleLabel: {
                        display: false,
                        labelString: 'Month'
                    },
                    ticks: {
                        fontColor: "#9aa0ac", // this here
                    }
                }],
                yAxes: [{
                    display: true,
                    gridLines: {
                        display: true,
                        drawBorder: false,
                        zeroLineColor: "transparent"
                    },
                    scaleLabel: {
                        display: false,
                        labelString: 'Value'
                    },
                    ticks: {
                        fontColor: "#9aa0ac", // this here
                    }
                }]
            },
            title: {
                display: false,
                text: 'Normal Legend'
            }
        }
    });

}

function desktopChart2(months, salProcessed, salFailed) {

    /* Bar Graph */
    $("#echart_graph_line").css("height", "300")
    var chart = document.getElementById('echart_graph_line');
    var barChart = echarts.init(chart);

    barChart.setOption({

        tooltip: {
            trigger: "axis"
        },
        legend: {
            data: ["Paid", "Un Paid"],
            textStyle: {
                color: '#9aa0ac'
            },
        },
        toolbox: {
            show: !1
        },
        calculable: !1,
        xAxis: [{
            type: "category",
            axisLabel: {
                color: '#9aa0ac'
            },
            data: months
        }],
        yAxis: [{
            type: "value",
            axisLabel: {
                color: '#9aa0ac'
            },
        }],
        series: [{
            name: "Paid",
            type: "bar",
            data: salProcessed,
        }, {
            name: "Un Paid",
            type: "bar",
            data: salFailed,

        }],
        color: ['#85C14D', '#7D7D7D']

    });


}
function initBedgeChart() {

    var draw = Chart.controllers.line.prototype.draw;
    Chart.controllers.line = Chart.controllers.line.extend({
        draw: function () {
            draw.apply(this, arguments);
            var ctx = this.chart.chart.ctx;
            var originalStroke = ctx.stroke
            ctx.stroke = function () {
                ctx.save();
                ctx.shadowColor = '#acaeaf';
                ctx.shadowBlur = 10;
                ctx.shadowOffsetX = 0;
                ctx.shadowOffsetY = 10;
                originalStroke.apply(this, arguments)
                ctx.restore();
            };
        }
    });


    var ctx = document.getElementById("infoboxChart1");
    ctx.height = 60;
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["01", "02", "03", "04"],
            datasets: [{
                data: [20, 65, 35, 75],
                borderWidth: 3,
                borderColor: "#D07BED",
                pointBackgroundColor: "#D07BED",
                pointBorderColor: "#D07BED",
                pointHoverBackgroundColor: "#FFF",
                pointHoverBorderColor: "#D07BED",
                pointRadius: 3,
                pointHoverRadius: 4,
                fill: !1
            }]
        },
        options: {
            responsive: !0,
            maintainAspectRatio: false,
            tooltips: {
                enabled: false
            },
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                }],
                yAxes: [{
                    display: false,
                }]
            }
        }
    });

    var ctx = document.getElementById("infoboxChart2");
    ctx.height = 60;
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["01", "02", "03", "04"],
            datasets: [{
                data: [20, 65, 35, 75],
                borderWidth: 3,
                borderColor: "#34bfa3",
                pointBackgroundColor: "#34bfa3",
                pointBorderColor: "#34bfa3",
                pointHoverBackgroundColor: "#FFF",
                pointHoverBorderColor: "#34bfa3",
                pointRadius: 3,
                pointHoverRadius: 4,
                fill: !1
            }]
        },
        options: {
            responsive: !0,
            maintainAspectRatio: false,
            tooltips: {
                enabled: false
            },
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                }],
                yAxes: [{
                    display: false,
                }]
            }
        }
    });

    var ctx = document.getElementById("infoboxChart3");
    ctx.height = 60;
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["01", "02", "03", "04"],
            datasets: [{
                data: [20, 65, 35, 75],
                borderWidth: 3,
                borderColor: "#FF9800",
                pointBackgroundColor: "#FF9800",
                pointBorderColor: "#FF9800",
                pointHoverBackgroundColor: "#FFF",
                pointHoverBorderColor: "#FF9800",
                pointRadius: 3,
                pointHoverRadius: 4,
                fill: !1
            }]
        },
        options: {
            responsive: !0,
            maintainAspectRatio: false,
            tooltips: {
                enabled: false
            },
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                }],
                yAxes: [{
                    display: false,
                }]
            }
        }
    });

    var ctx = document.getElementById("infoboxChart4");
    ctx.height = 60;
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["01", "02", "03", "04"],
            datasets: [{
                data: [20, 65, 35, 75],
                borderWidth: 3,
                borderColor: "#74C6FF",
                pointBackgroundColor: "#74C6FF",
                pointBorderColor: "#74C6FF",
                pointHoverBackgroundColor: "#FFF",
                pointHoverBorderColor: "#74C6FF",
                pointRadius: 3,
                pointHoverRadius: 4,
                fill: !1
            }]
        },
        options: {
            responsive: !0,
            maintainAspectRatio: false,
            tooltips: {
                enabled: false
            },
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                }],
                yAxes: [{
                    display: false,
                }]
            }
        }
    });
}
function chart1() {
    var options = {
        chart: {
            height: 350,
            type: 'bar',
        },
        plotOptions: {
            bar: {
                horizontal: false,
                endingShape: 'rounded',
                columnWidth: '55%',
            },
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        series: [{
            name: 'Net Profit',
            data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
        }, {
            name: 'Revenue',
            data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
        }, {
            name: 'Free Cash Flow',
            data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
        }],

        xaxis: {
            type: 'category',
            categories: ['jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
            labels: {
                style: {
                    colors: '#9aa0ac',
                }
            }
        },

        yaxis: {
            title: {
                text: '$ (thousands)',
                style: {
                    color: '#9aa0ac'
                }
            },
            labels: {
                style: {
                    color: '#9aa0ac',
                }
            }

        },
        fill: {
            opacity: 1

        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "$ " + val + " thousands"
                }
            }
        }
    }

    var chart = new ApexCharts(
        document.querySelector("#chart1"),
        options
    );

    chart.render();


}

function chart2() {
    var options = {
        chart: {
            height: 350,
            type: 'area',
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth'
        },
        series: [{
            name: 'Project 1',
            data: [31, 40, 28, 51, 42, 109, 100]
        }, {
            name: 'Project 2',
            data: [11, 32, 45, 32, 34, 52, 41]
        }],

        xaxis: {
            categories: ['jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
            labels: {
                style: {
                    colors: '#9aa0ac',
                }
            }
        },
        yaxis: {
            title: {
                text: '$ (thousands)',
                style: {
                    color: '#9aa0ac'
                }
            },
            labels: {
                style: {
                    color: '#9aa0ac',
                }
            }

        },

    }

    var chart = new ApexCharts(
        document.querySelector("#chart2"),
        options
    );

    chart.render();

}


function GetDashboardData() {
    var DEPARTMENT_ID = $("#DEPARTMENT_ID").val();
    if (DEPARTMENT_ID !== "") {
       
        var SUBDEPARTMENT_ID = $("#SUBDEPARTMENT_ID").val();

        $("#barChartId").empty();
        $("#barChartId").append('<canvas id="desktop-chart1"></canvas>');

        if (SUBDEPARTMENT_ID != "") {

            var data = DEPARTMENT_ID + "," + SUBDEPARTMENT_ID;

            $.ajax(
                {
                    type: 'POST',
                    dataType: 'JSON',
                    url: '/Dashboard/GetDashboardData',
                    data: { 'jsonInput': data },
                    success:
                        function (response) {
                            var data = (JSON.parse(response));
                            if (data != null) {
                                $("#PendingHiring").text(data.PendingHiring);
                                $("#TolalNewWorkforceCount30Days").text(data.TolalNewWorkforceCount30Days);
                                $("#TolalWorkforceCount").text(data.TolalWorkforceCount);
                                $("#TotalOnTraining").text(data.OnTraining);
                                console.log(JSON.parse(response));
                                var months = [];
                                var salProcessed = [];
                                var salFailed = [];
                                $.each(data.MonthAndTotals, function (key, value) {
                                    months.push(value.MonthName);
                                    salProcessed.push(value.Processed);
                                    salFailed.push(value.Failed);
                                });
                                desktopChart1(months, salProcessed, salFailed);
                                desktopChart2(months, salProcessed, salFailed)
                            }
                        },
                    error:
                        function (response) {
                            alert("Error: " + response);
                        }
                });
        }
    }
}