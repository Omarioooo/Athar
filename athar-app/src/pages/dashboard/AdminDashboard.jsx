import React, { useState } from "react";
import { Users, DollarSign, Heart, Check, X } from "lucide-react";
import {
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
} from "recharts";
import CharityCard from "../../components/charity/CharityCard";

// بيانات الرسم البياني
const donationsPerMonth = [
    { month: "يناير", total: 45000 },
    { month: "فبراير", total: 52000 },
    { month: "مارس", total: 48000 },
    { month: "أبريل", total: 61000 },
    { month: "مايو", total: 55000 },
    { month: "يونيو", total: 67000 },
];

// بيانات الجمعيات
const charitiesData = [
    {
        id: 1,
        name: "جمعية الخير",
        description: "مساعدة الفقراء",
        campaignsCount: 12,
        img: "https://via.placeholder.com/350",
    },
    {
        id: 2,
        name: "جمعية نور",
        description: "رعاية الأطفال",
        campaignsCount: 7,
        img: "https://via.placeholder.com/350",
    },
    {
        id: 3,
        name: "جمعية أمل",
        description: "دعم الأسر المحتاجة",
        campaignsCount: 4,
        img: "https://via.placeholder.com/350",
    },
];

// اللون الأساسي
const PRIMARY = "rgba(78, 182, 230, 0.927)";

const SuperAdminDashboard = () => {
    const [charities, setCharities] = useState(charitiesData);

    // موافقة جمعية
    const handleApprove = (id) => {
        alert(`تمت الموافقة على الجمعية رقم ${id}`);
        // هنا تقدر تبعتي API approve
    };

    // رفض جمعية
    const handleReject = (id) => {
        alert(`تم رفض الجمعية رقم ${id}`);
        // هنا تقدر تبعتي API reject
    };

    return (
        <div className="min-h-screen bg-gray-50 p-6 lg:p-10" dir="rtl">
            <h1 className="text-3xl font-bold mb-6" style={{ color: PRIMARY }}>
                لوحة تحكم السوبر أدمن
            </h1>

            {/* إحصائيات */}
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-10">
                <div className="bg-white rounded-2xl p-6 shadow border">
                    <div className="flex items-center gap-3 mb-4">
                        <Users className="w-7 h-7 text-primary" />
                        <p className="font-semibold">عدد الجمعيات</p>
                    </div>
                    <p className="text-3xl font-bold">{charities.length}</p>
                </div>

                <div className="bg-white rounded-2xl p-6 shadow border">
                    <div className="flex items-center gap-3 mb-4">
                        <DollarSign className="w-7 h-7 text-primary" />
                        <p className="font-semibold">إجمالي التبرعات</p>
                    </div>
                    <p className="text-3xl font-bold">875,400 جنيه</p>
                </div>

                <div className="bg-white rounded-2xl p-6 shadow border">
                    <div className="flex items-center gap-3 mb-4">
                        <Users className="w-7 h-7 text-primary" />
                        <p className="font-semibold">عدد المستخدمين</p>
                    </div>
                    <p className="text-3xl font-bold">15,420</p>
                </div>

                <div className="bg-white rounded-2xl p-6 shadow border">
                    <div className="flex items-center gap-3 mb-4">
                        <Heart className="w-7 h-7 text-primary" />
                        <p className="font-semibold">إعجابات الميديا</p>
                    </div>
                    <p className="text-3xl font-bold">42,680</p>
                </div>
            </div>

            {/* رسم بياني */}
            <div className="bg-white rounded-2xl p-8 shadow border mb-12">
                <h3
                    className="text-xl font-bold mb-6"
                    style={{ color: PRIMARY }}
                >
                    إجمالي التبرعات كل شهر
                </h3>

                <ResponsiveContainer width="100%" height={300}>
                    <AreaChart data={donationsPerMonth}>
                        <defs>
                            <linearGradient
                                id="donationsGradient"
                                x1="0"
                                y1="0"
                                x2="0"
                                y2="1"
                            >
                                <stop
                                    offset="0%"
                                    stopColor={PRIMARY}
                                    stopOpacity={0.5}
                                />
                                <stop
                                    offset="100%"
                                    stopColor={PRIMARY}
                                    stopOpacity={0}
                                />
                            </linearGradient>
                        </defs>

                        <CartesianGrid strokeDasharray="3 3" stroke="#eee" />
                        <XAxis dataKey="month" tick={{ fill: "#666" }} />
                        <YAxis tick={{ fill: "#666" }} />
                        <Tooltip />
                        <Area
                            type="monotone"
                            dataKey="total"
                            stroke={PRIMARY}
                            strokeWidth={3}
                            fill="url(#donationsGradient)"
                        />
                    </AreaChart>
                </ResponsiveContainer>
            </div>

            {/* الجمعيات */}
            <h2 className="text-2xl font-bold mb-6" style={{ color: PRIMARY }}>
                الجمعيات (بموافقة / رفض)
            </h2>

            <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
                {charities.map((c) => (
                    <div
                        key={c.id}
                        className="bg-white rounded-2xl p-5 shadow border relative"
                    >
                        {/* بطاقة الجمعية */}
                        <CharityCard
                            id={c.id}
                            name={c.name}
                            description={c.description}
                            campaignsCount={c.campaignsCount}
                            img={c.img}
                        />

                        {/* زرارين موافقة ورفض */}
                        <div className="flex gap-3 mt-4">
                            <button
                                onClick={() => handleApprove(c.id)}
                                className="flex-1 bg-green-500 text-white py-2 rounded-lg hover:bg-green-600"
                            >
                                <Check className="inline w-4 h-4 ml-1" /> موافقة
                            </button>

                            <button
                                onClick={() => handleReject(c.id)}
                                className="flex-1 bg-red-500 text-white py-2 rounded-lg hover:bg-red-600"
                            >
                                <X className="inline w-4 h-4 ml-1" /> رفض
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default SuperAdminDashboard;
