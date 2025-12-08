import { Link } from "react-router-dom";
import charities from "../assets/images/charity.jpg";
import { 
  ArrowLeft, Heart, Users, Target, Sparkles, Eye, Award, HandHeart, Building, Mail, Phone, MapPin 
} from "lucide-react";

const primaryColor = "rgba(78, 182, 230, 0.927)";
const primaryBg = "rgba(78, 182, 230, 0.1)"; // نسخة فاتحة للخلفيات

const AboutUs = () => {
  const values = [
    { icon: Heart, title: "الإخلاص", description: "نعمل بإخلاص تام لخدمة المحتاجين وضمان وصول كل تبرع لمستحقيه" },
    { icon: Users, title: "الشفافية", description: "نوفر تقارير دورية ومفصلة عن كل تبرع وكيف تم استخدامه" },
    { icon: Award, title: "الجودة", description: "نحرص على تقديم أفضل خدمة للجمعيات والمتبرعين على حد سواء" },
    { icon: HandHeart, title: "التعاون", description: "نؤمن بقوة العمل الجماعي والتعاون بين جميع الأطراف" },
  ];

  const stats = [
    { number: "500+", label: "جمعية خيرية مسجلة" },
    { number: "10,000+", label: "متبرع نشط" },
    { number: "5M+", label: "جنيه تبرعات" },
    { number: "100,000+", label: "مستفيد" },
  ];

  const chooseUs = [
    {
      icon: Building,
      title: "للجمعيات الخيرية",
      points: ["صفحة مخصصة لعرض مشاريعكم","أدوات لإدارة التبرعات","تقارير مفصلة عن الأداء","دعم تقني مجاني"],
    },
    {
      icon: Users,
      title: "للمتبرعين",
      points: ["اختيار الجمعية المناسبة","تتبع تبرعاتك","شهادات تبرع معتمدة","طرق دفع متعددة وآمنة"],
    },
    {
      icon: Sparkles,
      title: "للمجتمع",
      points: ["شفافية كاملة","تأثير حقيقي وملموس","تقارير دورية","مساهمة في التنمية المستدامة"],
    },
  ];

  return (
    <div dir="rtl" className="min-h-screen bg-gray-100 text-gray-800">
      {/* Header */}
      <header className="py-6 px-8 flex justify-between items-center border-b border-gray-300 bg-white">
        <Link to="/" className="flex items-center gap-2 text-gray-600 hover:text-gray-900 transition-colors">
          <ArrowLeft className="w-5 h-5" />
          العودة للرئيسية
        </Link>
        <h1 className="text-2xl font-bold" style={{ color: primaryColor }}>أثر</h1>
      </header>

     {/* Hero Section */}
<section className="py-20 px-8" style={{ backgroundColor: primaryBg }}>
  <div className="max-w-7xl mx-auto grid lg:grid-cols-2 gap-16 items-center">
    {/* Image */}
    <div className="h-64 w-full rounded-2xl overflow-hidden">
      <img 
        src={charities} 
        alt="أثر - جمعيات خيرية" 
        className="w-full h-full object-cover"
      />
    </div>

    {/* Content */}
    <div>
      <span className="inline-block px-4 py-2 rounded-full font-semibold text-sm mb-6" style={{ backgroundColor: primaryBg, color: primaryColor }}>من نحن</span>
      <h2 className="text-4xl font-bold mb-6" style={{ color: primaryColor }}>نحن منصة أثر للعمل الخيري</h2>
      <p className="text-lg mb-4">
        تأسست منصة "أثر" عام 2025 بهدف واحد واضح: ربط الجمعيات الخيرية الصغيرة بالمتبرعين الراغبين في إحداث فرق حقيقي.
      </p>
      <p className="text-lg">
        فريقنا مكون من متخصصين في التقنية والعمل الخيري، نعمل معاً لتوفير أفضل الأدوات للجمعيات الخيرية لتحقيق أهدافها.
      </p>
    </div>
  </div>
</section>
      {/* Our Values */}
      <section className="py-20 px-8">
        <div className="max-w-7xl mx-auto text-center mb-16">
          <h2 className="text-4xl font-bold mb-4" style={{ color: primaryColor }}>قيمنا</h2>
          <p className="text-gray-600">المبادئ التي نعمل بها</p>
        </div>
        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-8">
          {values.map((item, index) => (
            <div key={index} className="bg-white p-8 rounded-2xl shadow hover:shadow-lg transition-shadow">
              <div className="w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-6" style={{ backgroundColor: primaryBg }}>
                <item.icon className="w-8 h-8" style={{ color: primaryColor }} />
              </div>
              <h3 className="text-xl font-bold mb-3">{item.title}</h3>
              <p>{item.description}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Stats Section */}
      <section className="py-20 px-8" style={{ backgroundColor: primaryColor, color: "#fff" }}>
        <div className="max-w-7xl mx-auto text-center mb-12">
          <h2 className="text-3xl font-bold mb-4">إنجازاتنا بالأرقام</h2>
        </div>
        <div className="grid md:grid-cols-4 gap-8 text-center">
          {stats.map((stat, index) => (
            <div key={index}>
              <div className="text-5xl font-bold mb-2">{stat.number}</div>
              <div className="text-lg">{stat.label}</div>
            </div>
          ))}
        </div>
      </section>

      {/* Contact */}
      <section className="py-20 px-8">
        <div className="max-w-4xl mx-auto text-center">
          <h2 className="text-4xl font-bold mb-8" style={{ color: primaryColor }}>تواصل معنا</h2>
          <div className="grid md:grid-cols-3 gap-8 mb-12">
            <div className="flex flex-col items-center gap-3">
              <Mail className="w-6 h-6" style={{ color: primaryColor }} />
              info@athar.org
            </div>
            <div className="flex flex-col items-center gap-3">
              <Phone className="w-6 h-6" style={{ color: primaryColor }} />
              +20 123 456 7890
            </div>
            <div className="flex flex-col items-center gap-3">
              <MapPin className="w-6 h-6" style={{ color: primaryColor }} />
              القاهرة، مصر
            </div>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="py-10 px-8 bg-gray-800 text-white text-center">
        <div className="flex flex-col md:flex-row justify-center items-center gap-3">
          <Heart className="w-6 h-6" style={{ color: "red" }} />
          أثر © 2024. جميع الحقوق محفوظة.
        </div>
      </footer>
    </div>
  );
};

export default AboutUs;
