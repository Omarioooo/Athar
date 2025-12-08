import React from "react";
import "../../styles/scope.css";
import {
  Target,
  Users,
  DollarSign,
  Heart,
  Image,
  TrendingUp,
  Calendar,
  Eye,
  Bell,
  Search,
  Home,
  BarChart3
} from "lucide-react";

import {
  AreaChart,
  Area,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  BarChart,
  Bar
} from "recharts";

// البيانات
const donationsData = [
  { month: "يناير", donations: 45000, followers: 1200 },
  { month: "فبراير", donations: 52000, followers: 1450 },
  { month: "مارس", donations: 48000, followers: 1680 },
  { month: "أبريل", donations: 61000, followers: 1920 },
  { month: "مايو", donations: 55000, followers: 2100 },
  { month: "يونيو", donations: 67000, followers: 2400 },
];

const campaignsData = [
  { name: "غذاء", value: 35 },
  { name: "تعليم", value: 25 },
  { name: "صحة", value: 20 },
  { name: "أيتام", value: 20 },
 
];

const weeklyLikesData = [
  { day: "السبت", likes: 1200 },
  { day: "الأحد", likes: 1800 },
  { day: "الإثنين", likes: 2400 },
  { day: "الثلاثاء", likes: 1600 },
  { day: "الأربعاء", likes: 2800 },
  { day: "الخميس", likes: 3200 },
  { day: "الجمعة", likes: 2100 },
];

const stats = [
  { title: "إجمالي الحملات", value: "24", trend: 12, icon: Users },
  { title: "إجمالي التبرعات", value: "٨٧٥,٤٠٠ جنيه", trend: 8, icon: DollarSign },
  { title: "المتابعين", value: "15,420", trend: 15, icon: Users },
  { title: "إعجابات الميديا", value: "42,680", trend: 23, icon: Heart },
];

const bottomStats = [
  { title: "عدد الوسائط", value: "156", trend: 18, icon: Image },
  { title: "معدل النمو", value: "32%", trend: 5, icon: TrendingUp },
  { title: "الحملات النشطة", value: "8", trend: 2, icon: Target },
  { title: "المشاهدات", value: "128,450", trend: 34, icon: Eye },
];

// اللون الأزرق السماوي
const SKY_BLUE = "rgba(78, 182, 230, 0.927)";
const SKY_BLUE_LIGHT = "rgba(78, 182, 230, 0.6)";
const SKY_BLUE_DARK = "#3b82f6";

const Index = () => {
  return (
    <div class="tailwind-scope">
    <div className="min-h-screen bg-gradient-to-br from-sky-50 to-white" dir="rtl">
  

      <main className="p-6 lg:p-10">
        <div className="mb-10">
          
          <h1 className="text-gray-600 mt-2">إليك نظرة عامة على إحصائيات الجمعية</h1>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-10">
          {stats.map((stat, i) => (
            <div
              key={i}
              className="bg-white rounded-2xl p-6 shadow-lg hover:shadow-2xl transition-all duration-500 border border-gray-100 hover:border-sky-300 hover:-translate-y-2"
            >
              <div className="flex items-center justify-between mb-4">
                <div className="p-3 rounded-xl bg-sky-100 transition-colors">
                  <stat.icon className="w-7 h-7 text-sky-600" />
                </div>
                <span className="text-sm font-bold text-green-600 bg-green-100 px-3 py-1 rounded-full">
                  +{stat.trend}%
                </span>
              </div>
              <p className="text-gray-600 text-sm mb-1">{stat.title}</p>
              <p className="text-3xl font-bold text-gray-800">{stat.value}</p>
            </div>
          ))}
        </div>

        {/* Charts */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 mb-10">
          {/* Area Chart */}
          <div className="lg:col-span-2 bg-white rounded-2xl p-8 shadow-xl border border-gray-100">
            <h3 className="text-xl font-bold text-gray-800 mb-6">نمو التبرعات والمتابعين</h3>
            <ResponsiveContainer width="100%" height={350}>
              <AreaChart data={donationsData}>
                <defs>
                  <linearGradient id="donationsGradient" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0%" stopColor={SKY_BLUE} stopOpacity={0.6} />
                    <stop offset="100%" stopColor={SKY_BLUE} stopOpacity={0} />
                  </linearGradient>
                  <linearGradient id="followersGradient" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0%" stopColor="#10b981" stopOpacity={0.5} />
                    <stop offset="100%" stopColor="#10b981" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                <XAxis dataKey="month" tick={{ fill: "#666" }} />
                <YAxis tick={{ fill: "#666" }} />
                <Tooltip />
                <Area type="monotone" dataKey="donations" stroke={SKY_BLUE} strokeWidth={4} fill="url(#donationsGradient)" />
                <Area type="monotone" dataKey="followers" stroke="#10b981" strokeWidth={4} fill="url(#followersGradient)" />
              </AreaChart>
            </ResponsiveContainer>
          </div>

          {/* Pie Chart */}
          <div className="bg-white rounded-2xl p-8 shadow-xl border border-gray-100">
            <h3 className="text-xl font-bold text-gray-800 mb-6">توزيع الحملات</h3>
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={campaignsData}
                  cx="50%" cy="50%"
                  innerRadius={60} outerRadius={100}
                  paddingAngle={5}
                  dataKey="value"
                >
                  {campaignsData.map((_, index) => (
                    <Cell key={index} fill={[SKY_BLUE, "#10b981", "#f59e0b", "#8b5cf6"][index]} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
            <div className="grid grid-cols-2 gap-3 mt-6">
              {campaignsData.map((item, i) => (
                <div key={i} className="flex items-center gap-3">
                  <div className="w-4 h-4 rounded-full" style={{ backgroundColor: [SKY_BLUE, "#10b981", "#f59e0b", "#8b5cf6"][i] }} />
                  <span className="text-gray-700 text-sm">{item.name}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* Bar Chart + Small Stats */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          <div className="bg-white rounded-2xl p-8 shadow-xl border border-gray-100">
            <h3 className="text-xl font-bold text-gray-800 mb-6">الإعجابات الأسبوعية</h3>
            <ResponsiveContainer width="100%" height={350}>
              <BarChart data={weeklyLikesData}>
                <defs>
                  <linearGradient id="barGradient" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0%" stopColor={SKY_BLUE_LIGHT} />
                    <stop offset="100%" stopColor={SKY_BLUE} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                <XAxis dataKey="day" tick={{ fill: "#666" }} />
                <YAxis tick={{ fill: "#666" }} />
                <Tooltip />
                <Bar dataKey="likes" fill="url(#barGradient)" radius={[12, 12, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>

          <div className="grid grid-cols-2 gap-6">
            {bottomStats.map((stat, i) => (
              <div key={i} className="bg-white rounded-2xl p-6 shadow-lg hover:shadow-2xl transition border border-gray-100 hover:border-sky-300">
                <div className="flex items-center justify-between mb-4">
                  <div className="p-3 rounded-xl bg-sky-100">
                    <stat.icon className="w-6 h-6 text-sky-600" />
                  </div>
                  <span className="text-sm font-bold text-green-600 bg-green-100 px-3 py-1 rounded-full">+{stat.trend}%</span>
                </div>
                <p className="text-gray-600 text-sm">{stat.title}</p>
                <p className="text-2xl font-bold text-gray-800">{stat.value}</p>
              </div>
            ))}
          </div>
        </div>
      </main>
    </div>
    </div>
  );
};

export default Index;
