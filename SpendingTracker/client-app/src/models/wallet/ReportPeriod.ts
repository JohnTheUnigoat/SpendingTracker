interface ReportPeriod {
    text: string;
    code: string;
};

const reportPeriods: ReportPeriod[] = [
    {
        text: 'Current day',
        code: 'curr_day'
    },
    {
        text: 'Current week',
        code: 'curr_week'
    },
    {
        text: 'Current month',
        code: 'curr_month'
    },
    {
        text: 'Current year',
        code: 'curr_year'
    },
    {
        text: 'All time',
        code: 'all_time'
    },
    // {
    //     text: 'Custom',
    //     code: 'custom'
    // }
];

const getReportPeriod = (code: string) => {
    return reportPeriods.find(rp => rp.code === code) as ReportPeriod;
};

export type { ReportPeriod };

export {
    reportPeriods,
    getReportPeriod
};