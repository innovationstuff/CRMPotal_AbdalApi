using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TextController : ApiController
    {
        public HttpResponseMessage GetRolesandLaws()
        {
            var filePath = @"~\Inferstructures\filecontent.html";
            var fullText = File.ReadAllText(HostingEnvironment.MapPath(filePath));
            fullText = fullText.Replace("\n", "").Replace("\r", "");
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(fullText, System.Text.Encoding.UTF8, "text/plain");
            return resp;
        }

        public string GetNews(int lang = 0)
        {

            //            string s = @"
            //               كما يسعدنا إبلاغكم بأننا سنقوم بإطلاق نافذة تفاعلية للخدمات الخاصة والتي تتسم بتوفير خدمات  وقتية كطلب سباك أو فريق لتنظيف المنزل او الضيافة وسيتم الإعلان عن خدماتنا بالتفصيل من خلال نافذة الخدمات الخاصة قريبا 
            //
            //                <img src='/images/newsIcon.jpg' />
            //                    تعتزم شركة ناس للإستقدام إطلاق خدمتها الجديده 'يومك على كيفك' وهي خدمة مبتكرة بنظام الزيارات المجدولة في الشهر الواحد
            //
            //                <img src='/images/newsIcon.jpg' />";

            string s = @"
              
               كما يسعدنا إبلاغكم بأننا سنقوم بإطلاق نافذة تفاعلية للخدمات الخاصة والتي تتسم بتوفير خدمات  وقتية كطلب سباك أو فريق لتنظيف المنزل او الضيافة وسيتم الإعلان عن خدماتنا بالتفصيل من خلال نافذة الخدمات الخاصة قريبا 

                <img src='images/newsIcon.jpg' />
                    تعتزم شركة ناس للإستقدام إطلاق خدمتها الجديده 'يومك على كيفك' وهي خدمة مبتكرة بنظام الزيارات المجدولة في الشهر الواحد

                <img src='images/newsIcon.jpg' />";



            return s.Replace("\n", "").Replace("\r", "");
        }

        public string GetBussinessText(int lang = 0)
        {

            string s = @"
              <div>
                        <p>
                            تقدم الشركة الخدمات التالية للشركات والمؤسسات والقطاعات الحكومية وفقاً للتالي :
                        </p>
                        <p><strong> 1- حلوله مساندة:</strong> وهي عبارة عن توفير الكوادر البشرية المناسبة لطبيعة العمل لدى العميل وذلك تحت كفالة الشركة السعودية للاستقدام ويؤمن صاحب العمل السكن والنقل وجميع متطلبات العمل.            </p>
                        <p><strong>2-  حلول متكاملة:</strong> هي عبارة عن توفير الكوادر البشرية المناسبة للعميل مع توفير السكن والنقل والزي و أي مستلزمات أخرى تتناسب مع طبيعة عمل العميل.</p>
                        <p><strong>3-  تقوم الشركة بالتوسط</strong> نيابة عن صاحب العمل باستقدام العمالة المطلوبة من خلال تأشيرات صاحب العمل .</p>
                        <p><strong> 4- حلول شاملة:</strong> تقوم الشركة نيابة عن صاحب العمل بتنظيم وإدارة جميع الكوادر لدى العميل ضمن مشروع محدّد وبمدى زمني متفق عليه .</p>
                        <p><strong> 5- حلول موسمية:</strong> تقديم خدمات متكاملة لاختيار واستقدام كوادر متخصّصة في مجالات مختلفة لتقديم خدماتها في مواسم محدّدة .</p>
                        <p><strong> 6- حلول تطويرية:</strong> تقديم خدمات من خلال شراكات استراتيجية مع شركاء متخصصين في مجالات الاستشارات الإدارية وغيرها . </p>
                    </div>";



            return s.Replace("\n", "").Replace("\r", "");
        }

        public string GetSpecialservicesText(int lang = 0)
        {

            string s = @"
              <div style='font-size:medium'>
                        <p>
                            تقدم الشركة ناس  خدمة مناسبات لتوفير مضيفات ومؤهلات ومدربات بالساعة لمناسباتكم . تتوافق مع احتياجات العملاء وبتكلفة مناسبة وتشمل توصيل الخدمة الى المنزل وفق الوقت الذى يناسبهم و المتفق عليه وبشكل يحترم خصوصية وطبيعة العملاء والمجتمع ويحميهم من الاخطار الامنية والاخلاقية والصحية والقانونية  .
                        </p>
 <p><strong> الخدمة متاحة حالياعن طريق التواصل مع المركز الاتصال او زيارة اقرب فرع وقريبا من خلال التطبيق</strong></p>
                       
                    </div>";




            return s.Replace("\n", "").Replace("\r", "");
        }

        public string GetWhoweare(int lang = 0)
        {

            string s = @"
              <div class='pageTitle'>شركة ناس لحلول الموارد البشرية</div>
                <div class='text-center'>
هي شركة سعودية تقدم دعم الكوادر البشرية للغير والتي تلبي احتياجات سوق العمل من مختلف القطاعات والتخصصات والمهن والجنسيات وتحرص شركة ناس للاستقدام على اختيار العمالة المدربة ذات الكفاءة العالية. وحرصت الشركة على التواصل المباشر بالدول المصدرة للعمالة لتوفر لعملائها كل ما يتطلبه سوق العمل من متخصصين وحرفيين وعمالة منزلية ذات خبرات عالية. شركة ناس للاستقدام هي امتداد من خبرات وكوادر في مجال الاستقدام وتوفير العمالة المنزلية النسائية والعمالة الرجالية بجميع تخصصاتها وفئاتها المهنية وتأجير العمالة للشركات والمؤسسات بجميع التخصصات والقطاعات المهمة في الدولة .              </div>";




            return s.Replace("\n", "").Replace("\r", "");
        }

        public string GetFaqs(int lang = 0)
        {

            string s = @"
                <div class='pageTitle faqq'><i class='fa fa-question-circle'></i> ما هو 'موقع ناس' ؟</div>
        <div class='text-justify faqa'> ناس هو نافذة الخدمات التي تقدمها شركة ناس للخدمات العاملة وتوفير الايدى العاملة فى الممكلة العربية السعودية وتتمثل الخدمات فى خدمات قطاع اعمال وخدمات قطاع افراد و خدمات بالساعه          <div class='pageTitle faqq'><i class='fa fa-question-circle'></i> ما فائدته ؟</div>
            <div class='text-justify faqa'>
                ناس هو ببساطة يقدم عمالة منزلية بالساعه وتوفر لعملائها الحصول علي الخدمات بكل يسر وسهولة بدون الحاجة الى زيارة فروع الشركة .
                ناس : هى خدمة عاملات منزليات بالساعه وتصميم باقات باعلى مستويات الجودة والاتقان والرمونة وسرعه التواصل مع العملاء لتلبية احتيجاتهم المتنوعه من خلال باقات مختلفة وعروض مختلفة ايضا
            </div>
</div>
        <div class='text-justify faqa'></div>";




            return s.Replace("\n", "").Replace("\r", "");
        }

        public string GetContactUs(int lang = 0)
        {

            string s = @"
                <br />
                   
                    <div style='text-align:right'>
                        <div>
                            <p style='text-align:center'>شكرا للتواصل معنا يمكنكم التواصل  مع شركة ناس للموارد البشرية من خلال افرعنا.</p>
                            <br />
                            <div style=' text-align: center;'><strong><span class='pageTitle'>فروع شركة ناس</span> </strong></div>
                            <br />
                        </div>

                        <div>
                            <div>
                                <strong>  شركة ناس للاستقدام  الكيان الرئيسي </strong>
                                <p><a href='http://goo.gl/H8Ydov'>الموقع على الخريطة</a></p>
                                <p>
                                    <span style='color:blue'><strong> ارقام التواصل:</strong></span>
                                    <a href='tel:920000748'>920000748</a>
                                </p>
                            </div>

                            </>
                            <br />
                            <div> <strong>  شركة ناس للاستقدام فروع الرياض </strong></div>
                            <br />
                            <ul>



                                <li>
                                    <strong>  فرع الملك عبدالله </strong>
                                    <p><a href='https://goo.gl/OQ39XD'>الموقع على الخريطة</a></p>
                                   
                                </li>
                                <li>
                                    <strong>  فرع الملك عبدالعزيز</strong>
                                    <p><a href='http://goo.gl/H8Ydov'>الموقع على الخريطة</a></p>
                                  
                                </li>

                               
                              

                            </ul>

                            <br />
                            <div> <strong>  شركة ناس للاستقدام فرع الدمام </strong></div>
                            <br />
                            <ul>
                            <li>
                                <strong> فرع  طريق الملك فهد</strong>
                                <p><a href='https://goo.gl/upptxb'>الموقع على الخريطة</a></p>
                                
                            </li>
                            </ul>



                            <br />
                            <div> <strong>  شركة ناس للاستقدام فرع القصيم/ بريدة </strong></div>
                            <br />
                            <ul>
                               
                                <li>
                                    <strong>فرع شارع الملك عبد العزيز </strong>
                                    <p><a href='https://goo.gl/pg7wBA'>الموقع على الخريطة</a></p>
                  
                                </li>

                                
</ul>





                            <br />

                            <div style='text-align:center'>
                                <p><strong>او يمكنكم  التواصل معانا على البريد الإلكتروني </strong></p>
                                <div><a href='mailto:Info@naas.com.sa'>Info@naas.com.sa</a></div>
                            </div>

                        </div>




                    </div>";




            return s.Replace("\n", "").Replace("\r", "");
        }

        [HttpGet]
        public AlertModel AlertText(int lang = 0)
        {

            AlertModel alerttext = new AlertModel()
            {
                Locations = @"
 <p><B>السماح لتطبيق ناس للاستقدام لمعرفة بيانات موقعك عند استخدام التطبيق؟</B></p>


",
                Notifications = @"
 <p><B>السماح لتطبيق ناس للاستقدام لارسال اشعارت لك بالعروض الجديدة؟</B></p>

",

            };


            AlertModel alerttexten = new AlertModel()
            {
                Locations = @"
 <p><B>Allow 'Nas Manpower' to access your location while you use the app?</B></p>


",
                Notifications = @"
 <p><B>Allow 'NasManpower app' to send to send Notifications ?</B></p>

",

            };

            if (lang == 0)
                return alerttext;

            return alerttexten;


        }


    }


    public class AlertModel
    {
        public string Locations { get; set; }
        public string Notifications { get; set; }

    }
}
