using System;

namespace Aooshi
{
    /// <summary>
    /// �����������ʽ
    /// </summary>
    public class RegexString
    {
        /// <summary>
        /// ��ʾ�Ƿ�Ϊ����ʱ���������ʽ ��ʽΪ: 2000-01-01 00:00:00
        /// </summary>
        public const string IsDateTime = @"^[0-9]{4}\-[0-9]{1,2}\-[0-9]{1,2} [0-9]{2}\:[0-9]{2}\:[0-9]{2}$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ��ȷ�����ڸ�ʽ������ʽ ��ʽΪ: 2000-01-01
        /// </summary>
        public const string IsDate = @"^[0-9]{4}\-[0-9]{1,2}\-[0-9]{1,2}$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ��ȷ��ʱ���ʽ������ʽ ��ʽΪ: 00:00:00
        /// </summary>
        public const string IsTime = @"^[0-9]{2}\:[0-9]{2}\:[0-9]{2}$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ�ʼ���������ʽ
        /// </summary>
        public const string IsEmail = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ����������ʽ,����С��λ��
        /// </summary>
        public const string IsMoney = @"^[123456789]{1}\d{0,9}$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ����Ϊ����,��������С���������ֵ
        /// </summary>
        public const string IsJustMoney = @"^\d+(\.\d{1,2})?$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ�����λС����ֻ�����ֵ
        /// </summary>
        public const string IsMinusMoney = @"^(\-){1}\d+{1}(\.\d{1,2})?$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ�����ɸ��ҿɴ���λС���Ľ����ֵ
        /// </summary>
        public const string IsJMMoney = @"^(\-)?\d+{1}(\.\d{1,2})?$";

        /// <summary>
        /// �ж������ַ����Ƿ�����������ĸ�����
        /// </summary>
        public const string IsLetterAndNumber = "^[A-Za-z0-9]+$";

        /// <summary>
        /// �ж������ַ����Ƿ�Ϊ����
        /// </summary>
        public const string IsChinese = @"[\u4e00-\u9fa5]+";

        /// <summary>
        /// �ж��Ƿ�Ϊ��ȷ��ҳ��, 0 - 999999 һ����λ����ҳ���������ʽ
        /// </summary>
        public const string IsPage = @"^\d{1,6}$";
        ///// <summary>
        ///// ��ʾ�Ƿ�Ϊ<seealso cref="MyRandom.CreateRanID()"/>�������ɵ������
        ///// </summary>
        //public const string IsID = @"^\d{15}$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ<seealso cref="RandomString.CreateOnlyID()"/>�������ɵ���Ч��2000����2700��֮���10λ�����
        /// </summary>
        public const string IsOnlyID = @"^[0-9A-Z]{10}$";

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ<seealso cref="RandomString.CreateCourseID()"/>��������������Ч�ַ���
        /// </summary>
        public const string IsCourseID = @"^[0-9A-Z]+$";

        /// <summary>
        /// ��֤�Ƿ�Ϊ��ȷ��32λMD5���ܴ�
        /// </summary>
        public const string IsMD5 = "^[0-9a-zA-Z]{32}$";
    }
}
