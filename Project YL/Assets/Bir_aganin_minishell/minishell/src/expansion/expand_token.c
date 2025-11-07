/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expand_token.c                                     :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/20 19:27:18 by marcudos          #+#    #+#             */
/*   Updated: 2025/05/27 17:27:25 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

t_args	*expand_token(char *input, t_env *env, t_shell *sh)
{
	t_exp	exp;
	t_args	*result;

	init_expander(&exp, input, env);
	while (input[exp.i])
		handle_next_token(&exp, input, sh);
	if (exp.prefix)
		add_token(exp.head, exp.prefix);
	result = *exp.head;
	free(exp.head);
	if (exp.prefix)
		free (exp.prefix);
	return (result);
}

void	handle_next_token(t_exp *exp, char *input, t_shell *sh)
{
	char	*key;

	if (input[exp->i] == '$')
	{
		if (input[exp->i + 1] == '?')
		{
			expand_status(exp, sh->last_status);
			exp->i = exp->i + 2;
			return ;
		}
		if (handle_special_expand(exp, input))
			return ;
		key = extract_key(input, &exp->i);
		if (key)
		{
			expand_variable(exp, key);
			free(key);
		}
	}
	else
	{
		exp->cur = start_prefix(input, &exp->i, &exp->state);
		exp->prefix = ft_strjoin_free(exp->prefix, exp->cur);
		exp->cur = NULL;
	}
}

int	handle_special_expand(t_exp *exp, char *input)
{
	char	*temp;

	if (input[exp->i + 1] == '\0' || input[exp->i + 1] == ' '
		|| input[exp->i + 1] == '\"' || input[exp->i + 1] == '\''
		|| input[exp->i + 1] == '/')
	{
		exp->cur = ft_strdup("$");
		if (!exp->cur)
			return (1);
		if (!exp->prefix)
			exp->prefix = ft_strdup("");
		temp = exp->prefix;
		exp->prefix = ft_strjoin(temp, exp->cur);
		free(temp);
		free(exp->cur);
		exp->cur = NULL;
		exp->i++;
		return (1);
	}
	return (0);
}

void	init_expander(t_exp *exp, char *input, t_env *env)
{
	exp->i = 0;
	exp->state = NO_QUOTE;
	exp->head = (t_args **) malloc(sizeof(t_args *));
	*exp->head = NULL;
	exp->prefix = start_prefix(input, &exp->i, &exp->state);
	exp->cur = NULL;
	exp->env = env;
}
